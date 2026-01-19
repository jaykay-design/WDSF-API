from datetime import datetime
from enum import StrEnum

from pydantic import BaseModel, Field, ConfigDict


class CompetitionStatus(StrEnum):
    PRE_REGISTRATION = "PreRegistration"
    REGISTERING = "Registering"
    REGISTRATION_CLOSED = "RegistrationClosed"
    PROCESSING = "Processing"
    CLOSED = "Closed"
    CANCELED = "Canceled"


class CompetitionDivision(StrEnum):
    GENERAL = "General"
    PROFESSIONAL = "Professional"


class Competition(BaseModel):
    model_config = ConfigDict(validate_by_name=True)
    id: int
    name: str
    last_modified_date: datetime = Field(validation_alias="lastmodifiedDate")
