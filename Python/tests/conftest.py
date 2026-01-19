from base64 import b64encode

import pytest
from pydantic import HttpUrl, SecretStr

from wdsf_python_sdk.config import (
    BasicAuthCredentials,
    HttpConfig,
    WdsfHttpClientConfig,
)
from wdsf_python_sdk.lib.http_client import HttpClient

auth = b64encode(b":".join(("some-user".encode(), "some-password".encode()))).decode()

EXPECTED_HEADERS = {
    "Accept": "application/json",
    "Authorization": f"Basic {auth}",
}


@pytest.fixture
def config() -> WdsfHttpClientConfig:
    return WdsfHttpClientConfig(
        url=HttpUrl("http://some-url.com"),
        auth=BasicAuthCredentials(
            username="some-user", password=SecretStr("some-password")
        ),
        http=HttpConfig(timeout=2),
    )


@pytest.fixture
def http_client(config: WdsfHttpClientConfig) -> HttpClient:
    return HttpClient(
        config=config.http,
        url=config.url,
        auth=(config.auth.username, str(config.auth.password.get_secret_value())),
    )
