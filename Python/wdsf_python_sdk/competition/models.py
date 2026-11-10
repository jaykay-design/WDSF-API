from datetime import date as date_
from datetime import datetime
from decimal import Decimal
from enum import StrEnum

from pydantic import BaseModel, ConfigDict, Field


class CompetitionStatus(StrEnum):
    """Every status the v2 API returns. The status filter accepts PreRegistration to
    PostponedTBC."""

    UNKNOWN = "Unknown"
    PRE_REGISTRATION = "PreRegistration"
    REGISTERING = "Registering"
    REGISTRATION_CLOSED = "RegistrationClosed"
    IN_PROGRESS = "InProgress"
    CLOSED = "Closed"
    CANCELED = "Canceled"
    PROCESSING = "Processing"
    POSTPONED = "Postponed"
    POSTPONED_TBC = "PostponedTBC"
    PROVISIONAL_EPC = "ProvisionalEPC"
    PROVISIONAL = "Provisional"
    ANY = "Any"


class CompetitionDivision(StrEnum):
    """Every division the v2 API returns. The division filter accepts General and
    Professional only."""

    UNKNOWN = "Unknown"
    GENERAL = "General"
    PROFESSIONAL = "Professional"
    HIP_HOP = "HipHop"
    WRRC = "WRRC"
    DISCO = "Disco"
    CARIBBEAN = "Caribbean"
    SMOOTH = "Smooth"
    BREAKING = "Breaking"
    SOLO_SYNCHRO_CHOREO = "SoloSyncroChoreo"
    STAGE = "Stage"
    E_SPORTS = "ESports"
    OTHER = "Other"
    ANY = "Any"


class Competition(BaseModel):
    model_config = ConfigDict(validate_by_name=True)
    id: int
    name: str
    last_modified_date: datetime = Field(validation_alias="lastmodifiedDate")
    discipline: str | None = None
    dance_form: str | None = Field(default=None, validation_alias="danceform")
    age: str | None = None
    coefficient: Decimal | None = None
    date: date_ | None = None
    status: CompetitionStatus | None = None
    division: CompetitionDivision | None = None
    country: str | None = None
    location: str | None = None
    competition_type: str | None = Field(
        default=None, validation_alias="competitionType"
    )
