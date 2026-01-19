import os.path
from typing import Any
from urllib.parse import urljoin

from pydantic import HttpUrl
from requests import Session

from wdsf_python_sdk.config import HttpConfig


class HttpClient:
    def __init__(self, config: HttpConfig, url: HttpUrl, auth: tuple[str, str]) -> None:
        self.session = self._get_session(auth=auth)
        self.base_url = str(url)
        self.config = config

    def _get_session(self, auth: tuple[str, str]) -> Session:
        session = Session()
        session.auth = auth
        session.headers.update({"Accept": "application/json"})
        return session

    def get(self, url: str, params: dict[str, Any] | None = None) -> dict[str, Any]:
        url = urljoin(self.base_url, url)

        response = self.session.get(
            url=str(url), params=params, timeout=self.config.timeout
        )

        response.raise_for_status()
        return response.json()

    def build_url(self, *paths: str) -> str:
        return os.path.join(self.base_url, *paths)
