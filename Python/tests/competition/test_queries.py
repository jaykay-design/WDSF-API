from wdsf_python_sdk.competition.queries import CompetitionQuerySerializer


class TestCompetitionQuerySerializer:
    def test_serialize_returns_expected(
        self, competition_query, competition_query_json
    ):
        # Then
        assert (
            CompetitionQuerySerializer.serialize(obj=competition_query)
            == competition_query_json
        )
