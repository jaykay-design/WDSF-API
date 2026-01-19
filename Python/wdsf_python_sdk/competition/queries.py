from datetime import datetime
from dataclasses import dataclass

from wdsf_python_sdk.competition.models import CompetitionStatus, CompetitionDivision
from wdsf_python_sdk.lib.datetime import serialize_date, serialize_datetime
from wdsf_python_sdk.lib.serialisation import Serializer


@dataclass(frozen=True, kw_only=True)
class CompetitionQuery:
    from_: datetime | None = None
    to: datetime | None = None
    modified_since: datetime | None = None
    world_ranking: bool | None = None
    division: CompetitionDivision | None = None
    status: CompetitionStatus | None = None
    location: str | None = None


class CompetitionQuerySerializer(Serializer[CompetitionQuery]):
    @classmethod
    def serialize(cls, obj: CompetitionQuery) -> dict[str, str]:
        serialised = {}
        if obj.from_:
            serialised["from"] = serialize_date(obj.from_)
        if obj.to:
            serialised["to"] = serialize_date(obj.to)
        if obj.modified_since:
            serialised["modifiedsince"] = serialize_datetime(obj.modified_since)
        if obj.world_ranking:
            serialised["worldranking"] = obj.world_ranking
        if obj.division:
            serialised["division"] = obj.division
        if obj.status:
            serialised["status"] = obj.status
        if obj.location:
            serialised["location"] = obj.location
        return serialised
