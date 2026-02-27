from wdsf_python_sdk.competition.details.models import CompetitionDetails
from wdsf_python_sdk.lib.http_client import HttpClient


class CompetitionDetailsProvider:
    def __init__(self, client: HttpClient) -> None:
        self.client = client

    def get_competition_details(self, competition_id: int) -> CompetitionDetails:
        url = self.client.build_url("competition", str(competition_id))
        response_json = self.client.get(url=url)
        return CompetitionDetails.model_validate(response_json)
