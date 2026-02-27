from wdsf_python_sdk.competition.models import Competition
from wdsf_python_sdk.competition.queries import (
    CompetitionQuery,
    CompetitionQuerySerializer,
)
from wdsf_python_sdk.lib.http_client import HttpClient


class CompetitionProvider:
    def __init__(self, client: HttpClient) -> None:
        self.client = client

    def get_competitions_by_query(self, query: CompetitionQuery) -> list[Competition]:
        url = self.client.build_url("competition")
        params = CompetitionQuerySerializer.serialize(obj=query)
        response_json = self.client.get(url=url, params=params)
        return [Competition.model_validate(comp) for comp in response_json]
