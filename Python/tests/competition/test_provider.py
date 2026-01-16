import os
from datetime import datetime
from typing import Any

import pytest
import responses
from responses import matchers

from tests.conftest import EXPECTED_HEADERS
from wdsf_python_sdk.competition.models import (
    CompetitionDivision,
    CompetitionStatus,
    Competition,
)
from wdsf_python_sdk.competition.provider import CompetitionProvider
from wdsf_python_sdk.competition.queries import CompetitionQuery
from wdsf_python_sdk.config import WdsfHttpClientConfig
from wdsf_python_sdk.lib.http_client import HttpClient


class TestCompetitionProvider:
    @pytest.fixture
    def provider(self, http_client: HttpClient) -> CompetitionProvider:
        return CompetitionProvider(client=http_client)

    def setup_competitions_query_response(
        self, config: WdsfHttpClientConfig, expected_params: dict[str, Any]
    ) -> None:
        responses.get(
            url=os.path.join(str(config.url), "competition"),
            match=[
                matchers.header_matcher(EXPECTED_HEADERS),
                matchers.query_param_matcher(params=expected_params),
            ],
            json=[
                {
                    "link": [
                        {"href": "some-competition-link", "rel": "self"},
                    ],
                    "id": 1,
                    "name": "comp-name-1",
                    "lastmodifiedDate": "2024-01-01T01:02:03",
                },
                {
                    "id": 2,
                    "name": "comp-name-2",
                    "lastmodifiedDate": "2024-02-01T11:12:13",
                },
            ],
        )

    @responses.activate
    def test_get_competitions_by_query_returns_expected_competitions(
        self, config, provider
    ):
        # Given
        query = CompetitionQuery(
            to=datetime(2024, 2, 1),
            from_=datetime(2024, 1, 1),
            location="some-location",
            world_ranking=True,
            modified_since=datetime(2024, 1, 1),
            division=CompetitionDivision.GENERAL,
            status=CompetitionStatus.CLOSED,
        )
        expected_params = {
            "to": "2024/02/01",
            "from": "2024/01/01",
            "location": "some-location",
            "worldranking": True,
            "modifiedsince": "2024/01/01T00:00:00",
            "division": "General",
            "status": "Closed",
        }

        self.setup_competitions_query_response(
            config=config, expected_params=expected_params
        )

        # When
        result = provider.get_competitions_by_query(query=query)

        # Then
        assert result == [
            Competition(
                id=1,
                name="comp-name-1",
                last_modified_date=datetime(2024, 1, 1, 1, 2, 3),
            ),
            Competition(
                id=2,
                name="comp-name-2",
                last_modified_date=datetime(2024, 2, 1, 11, 12, 13),
            ),
        ]
