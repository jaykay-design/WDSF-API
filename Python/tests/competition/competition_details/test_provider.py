import os
from datetime import datetime, date
from decimal import Decimal

import pytest
import responses
from responses import matchers

from tests.conftest import EXPECTED_HEADERS
from wdsf_python_sdk.competition.models import CompetitionDivision, CompetitionStatus
from wdsf_python_sdk.competition.details.models import CompetitionDetails
from wdsf_python_sdk.competition.details.provider import (
    CompetitionDetailsProvider,
)
from wdsf_python_sdk.config import WdsfHttpClientConfig
from wdsf_python_sdk.lib.http_client import HttpClient


class TestCompetitionDetailsProvider:
    @pytest.fixture
    def provider(self, http_client: HttpClient) -> CompetitionDetailsProvider:
        return CompetitionDetailsProvider(client=http_client)

    def setup_competition_details_response(
        self, config: WdsfHttpClientConfig, competition_id: int
    ) -> None:
        responses.get(
            url=os.path.join(str(config.url), f"competition/{competition_id}"),
            match=[
                matchers.header_matcher(EXPECTED_HEADERS),
            ],
            json={
                "id": competition_id,
                "discipline": "Latin",
                "age": "Adult",
                "coefficient": "1.25",
                "date": "2024-03-15",
                "division": "Professional",
                "country": "Germany",
                "location": "Berlin",
                "type": "World Championship",
                "lastmodifiedDate": "2024-01-01T01:02:03",
                "status": "Closed",
            },
        )

    @responses.activate
    def test_get_competition_details_returns_expected_competition_details(
        self, config, provider
    ):
        # Given
        competition_id = 123
        self.setup_competition_details_response(
            config=config, competition_id=competition_id
        )

        # When
        result = provider.get_competition_details(competition_id=competition_id)

        # Then
        assert result == CompetitionDetails(
            id=123,
            discipline="Latin",
            age="Adult",
            coefficient=Decimal("1.25"),
            date=date(2024, 3, 15),
            division=CompetitionDivision.PROFESSIONAL,
            country="Germany",
            location="Berlin",
            type="World Championship",
            last_modified_date=datetime(2024, 1, 1, 1, 2, 3),
            status=CompetitionStatus.CLOSED,
        )
