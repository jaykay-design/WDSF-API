# Python SDK for WDSF API

This SDK allows querying WDSF api, powered by strong typing and clear communication interface

## Installation

```bash
poetry add wdsf-python-sdk
```

## Usage

```python
from datetime import datetime
from pydantic import HttpUrl, SecretStr

from wdsf_python_sdk.config import (
    BasicAuthCredentials,
    HttpConfig,
    WdsfHttpClientConfig,
)
from wdsf_python_sdk.lib.http_client import HttpClient
from wdsf_python_sdk.competition.provider import CompetitionProvider
from wdsf_python_sdk.competition.queries import CompetitionQuery

# Create configuration
config = WdsfHttpClientConfig(
    url=HttpUrl("https://api.worlddancesport.org"),
    auth=BasicAuthCredentials(
        username="your-username",
        password=SecretStr("your-password")
    ),
    http=HttpConfig(timeout=10)
)

# Create HTTP client
client = HttpClient(
    config=config.http,
    url=config.url,
    auth=(config.auth.username, str(config.auth.password.get_secret_value()))
)

# Create provider
provider = CompetitionProvider(client=client)

# Query competitions
query = CompetitionQuery(
    from_=datetime(2024, 1, 1),
    to=datetime(2024, 12, 31)
)

competitions = provider.get_competitions_by_query(query)
```

## Development

Install dependencies
```bash
  poetry install
```

Activate shell
```bash
poetry shell
# Or
eval $(poetry env activate)
```

Run formatting
```bash
make fmt
```

Run style check & tests
```bash
make
```

# Documentation

For documentation of API parameters, see Project's Wiki

# Testing

See [Testing section](https://github.com/jaykay-design/WDSF-API/wiki/Accessing-the-service#testing) for API testing