from pydantic import BaseModel, HttpUrl, SecretStr


class BasicAuthCredentials(BaseModel):
    username: str
    password: SecretStr


class HttpConfig(BaseModel):
    timeout: int = 10


class WdsfHttpClientConfig(BaseModel):
    url: HttpUrl
    auth: BasicAuthCredentials
    http: HttpConfig = HttpConfig()
