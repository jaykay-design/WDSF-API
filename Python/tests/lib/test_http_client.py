import base64
import json
import os
import time

import pytest
import responses
from pytest_httpserver import HTTPServer
from requests import HTTPError, ReadTimeout
from responses import matchers
from werkzeug import Response

from wdsf_python_sdk.config import WdsfHttpClientConfig
from wdsf_python_sdk.lib.http_client import HttpClient


class TestHttpClient:
    @pytest.fixture
    def http_client(self, config) -> HttpClient:
        return HttpClient(
            config=config.http,
            url=config.url,
            auth=(config.auth.username, str(config.auth.password.get_secret_value())),
        )

    @pytest.fixture
    def httpserver_client(self, httpserver: HTTPServer, config) -> HttpClient:
        return HttpClient(
            config=config.http,
            url=httpserver.url_for(""),
            auth=(config.auth.username, str(config.auth.password.get_secret_value())),
        )

    @pytest.fixture
    def endpoint_url(self, config) -> str:
        return os.path.join(str(config.url), "some-endpoint")

    def timeout_response(self, config: WdsfHttpClientConfig) -> None:
        def delayed_response(request):
            import time

            time.sleep(config.http.timeout + 1)
            return 200, {}, ""

        responses.add_callback(
            method=responses.GET,
            url=str(config.url),
            callback=delayed_response,
        )

    def test_init_creates_expected_session(self, http_client, config):
        # Then
        assert http_client.session.headers["Accept"] == "application/json"
        assert http_client.session.auth == (
            config.auth.username,
            str(config.auth.password.get_secret_value()),
        )

    @responses.activate
    def test_get_makes_expected_request(self, http_client, config, endpoint_url):
        # Given
        expected_response = {"some": "data"}
        responses.add(
            method=responses.GET,
            url=endpoint_url,
            json=expected_response,
            match=[matchers.header_matcher({"Accept": "application/json"})],
            status=200,
        )

        # When
        response = http_client.get(url="some-endpoint")

        # Then
        assert len(responses.calls) == 1
        assert response == expected_response

    @responses.activate
    def test_get_raises_errors_from_server(self, http_client, config, endpoint_url):
        # Given
        expected_response = {"error": "some-error"}
        responses.add(
            method=responses.GET,
            url=endpoint_url,
            json=expected_response,
            status=400,
            match=[matchers.header_matcher({"Accept": "application/json"})],
        )

        # Then
        with pytest.raises(HTTPError) as e:
            http_client.get("some-endpoint")
        assert e.value.response.json() == expected_response

    def test_get_raises_timeout(self, httpserver, httpserver_client, config):
        # Given
        def delayed_response(request):
            time.sleep(3)
            return Exception("Should fail with timeout")

        httpserver.expect_request("/some-endpoint").respond_with_handler(
            delayed_response
        )

        # Then
        with pytest.raises(ReadTimeout):
            httpserver_client.get(url="some-endpoint")

    def test_get_sends_basic_auth_credentials(
        self, httpserver, httpserver_client, config
    ):
        # Given
        valid_credentials = base64.b64encode(b"some-user:some-password").decode("utf-8")

        def delayed_response(request):
            assert request.headers["Authorization"] == f"Basic {valid_credentials}"
            return Response(
                json.dumps({"response": "response"}),
                status=200,
                mimetype="application/json",
            )

        httpserver.expect_request("/some-endpoint").respond_with_handler(
            delayed_response
        )

        # When
        response = httpserver_client.get(url="some-endpoint")

        # Then
        assert response == {
            "response": "response",
        }

    @pytest.mark.parametrize(
        "paths,expected_suffix",
        [
            ([], ""),
            (["competitions"], "competitions"),
            (["competitions", "123", "results"], "competitions/123/results"),
            (["competitions/", "123/"], "competitions/123/"),
        ],
    )
    def test_build_url(self, http_client, config, paths, expected_suffix):
        url = http_client.build_url(*paths)
        assert url == f"{config.url}{expected_suffix}"
