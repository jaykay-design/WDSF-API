from datetime import datetime

import pytest

from wdsf_python_sdk.competition.queries import CompetitionQuery
from wdsf_python_sdk.competition.models import CompetitionStatus, CompetitionDivision


@pytest.fixture
def competition_query() -> CompetitionQuery:
    return CompetitionQuery(
        from_=datetime(2024, 1, 1),
        to=datetime(2024, 1, 2),
        division=CompetitionDivision.GENERAL,
        location="some-location",
        world_ranking=True,
        modified_since=datetime(2024, 1, 3, 12, 40, 45),
        status=CompetitionStatus.PROCESSING,
    )


@pytest.fixture
def competition_query_json() -> dict[str, str]:
    return {
        "from": "2024/01/01",
        "to": "2024/01/02",
        "division": "General",
        "location": "some-location",
        "worldranking": True,
        "modifiedsince": "2024/01/03T12:40:45",
        "status": "Processing",
    }
