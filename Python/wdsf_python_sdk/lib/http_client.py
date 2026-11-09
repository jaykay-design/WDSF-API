import posixpath
from typing import Any
from urllib.parse import urljoin

from pydantic import HttpUrl
from requests import Session

from wdsf_python_sdk.config import HttpConfig


API_KEY_HEADER = "X-WDSF-API-Key"


class HttpClient:
    def __init__(self, config: HttpConfig, url: HttpUrl, api_key: str) -> None:
        self.session = self._get_session(api_key=api_key)
        self.base_url = str(url)
        self.config = config

    def _get_session(self, api_key: str) -> Session:
        session = Session()
        session.headers.update({"Accept": "application/json", API_KEY_HEADER: api_key})
        return session

    def get(self, url: str, params: dict[str, Any] | None = None) -> dict[str, Any]:
        url = urljoin(self.base_url, url)

        response = self.session.get(
            url=str(url), params=params, timeout=self.config.timeout
        )

        response.raise_for_status()
        return response.json()

    def build_url(self, *paths: str) -> str:
        # URL paths use "/" on every OS (os.path.join gives "\\" on Windows).
        return posixpath.join(self.base_url, *paths)
