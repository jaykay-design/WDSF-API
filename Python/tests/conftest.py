import pytest
from pydantic import HttpUrl, SecretStr

from wdsf_python_sdk.config import (
    HttpConfig,
    WdsfHttpClientConfig,
)
from wdsf_python_sdk.lib.http_client import HttpClient

EXPECTED_HEADERS = {
    "Accept": "application/json",
    "X-WDSF-API-Key": "some-api-key",
}


@pytest.fixture
def config() -> WdsfHttpClientConfig:
    return WdsfHttpClientConfig(
        url=HttpUrl("http://some-url.com"),
        api_key=SecretStr("some-api-key"),
        http=HttpConfig(timeout=2),
    )


@pytest.fixture
def http_client(config: WdsfHttpClientConfig) -> HttpClient:
    return HttpClient(
        config=config.http,
        url=config.url,
        api_key=config.api_key.get_secret_value(),
    )
