from datetime import datetime, date
from decimal import Decimal

from pydantic import BaseModel, Field, ConfigDict

from ..models import CompetitionStatus, CompetitionDivision


class CompetitionDetails(BaseModel):
    model_config = ConfigDict(validate_by_name=True)
    id: int
    discipline: str
    age: str
    coefficient: Decimal
    date: date
    division: CompetitionDivision
    country: str
    location: str
    type: str
    last_modified_date: datetime = Field(validation_alias="lastmodifiedDate")
    status: CompetitionStatus
