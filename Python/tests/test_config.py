from pydantic import HttpUrl, SecretStr

from wdsf_python_sdk.config import (
    WdsfHttpClientConfig,
    BasicAuthCredentials,
    HttpConfig,
)


def test_load_wdsf_client_config_returns_expected_config(config):
    # Then
    assert config == WdsfHttpClientConfig(
        url=HttpUrl("http://some-url.com"),
        auth=BasicAuthCredentials(
            username="some-user", password=SecretStr("some-password")
        ),
        http=HttpConfig(timeout=2),
    )


def test_load_wdsf_config_loads_expected_default_http_config(config):
    # Then
    assert config.http == HttpConfig(timeout=2)
