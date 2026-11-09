from pydantic import BaseModel, HttpUrl, SecretStr


class HttpConfig(BaseModel):
    timeout: int = 10


class WdsfHttpClientConfig(BaseModel):
    url: HttpUrl
    # An API token from My WDSF (My API Tokens), sent as the X-WDSF-API-Key header.
    api_key: SecretStr
    http: HttpConfig = HttpConfig()
