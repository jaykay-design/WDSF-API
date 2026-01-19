from abc import ABC, abstractmethod
from typing import Generic, TypeVar, Any

T = TypeVar("T")


class Serializer(ABC, Generic[T]):
    @classmethod
    @abstractmethod
    def serialize(cls, obj: T) -> dict[str, Any]:
        pass


class Deserializer(ABC, Generic[T]):
    @classmethod
    @abstractmethod
    def deserialize(cls, obj: dict[str, Any]) -> T:
        pass
