from datetime import datetime

import pytest

from wdsf_python_sdk.lib.datetime import serialize_date, serialize_datetime


@pytest.mark.parametrize(
    argnames=["dt", "expected"],
    argvalues=[
        (datetime(2024, 1, 1, 12, 10, 42), "2024/01/01"),
        (datetime(2025, 12, 10), "2025/12/10"),
    ],
)
def test_serialize_date_returns_expected(dt, expected):
    # Then
    assert serialize_date(dt=dt) == expected


@pytest.mark.parametrize(
    argnames=["dt", "expected"],
    argvalues=[
        (datetime(2024, 1, 1, 12, 10, 42), "2024/01/01T12:10:42"),
        (datetime(2025, 12, 10), "2025/12/10T00:00:00"),
    ],
)
def test_serialize_datetime_returns_expected(dt, expected):
    # Then
    assert serialize_datetime(dt=dt) == expected
