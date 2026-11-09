from datetime import datetime, date
from decimal import Decimal

from pydantic import BaseModel, Field, ConfigDict

from ..models import CompetitionStatus, CompetitionDivision


class CompetitionDetails(BaseModel):
    model_config = ConfigDict(validate_by_name=True)
    id: int
    name: str | None = None
    discipline: str
    dance_form: str | None = Field(default=None, validation_alias="danceform")
    age: str
    coefficient: Decimal
    date: date
    division: CompetitionDivision
    country: str
    location: str
    type: str
    last_modified_date: datetime = Field(validation_alias="lastmodifiedDate")
    status: CompetitionStatus
    event_id: int | None = Field(default=None, validation_alias="eventId")
    group_id: int | None = Field(default=None, validation_alias="groupId")
