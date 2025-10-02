# Доменная модель модуля оценок

Диаграмма показывает предполагаемые агрегаты, сущности и value object'ы, удовлетворяющие правилам EVL-R-001 — EVL-R-014. Все связи отображают владение или использование объектов внутри предметной области.

```mermaid
classDiagram
    %% Identifiers
    class FormId {
        <<Value Object>>
        +Guid Value
    }

    class GroupId {
        <<Value Object>>
        +Guid Value
    }

    class CriterionId {
        <<Value Object>>
        +Guid Value
    }

    %% Form Metadata
    class FormCode {
        <<Value Object>>
        +string Value
    }

    class FormName {
        <<Value Object>>
        +string Value
    }

    class FormDescription {
        <<Value Object>>
        +string Value
    }

    class Tags {
        <<Value Object>>
        +ImmutableHashSet~Tag~ Items
    }

    class Tag {
        <<Value Object>>
        +string Value
    }

    class ITags {
        <<Interface>>
        +Tags Add(Tag tag)
        +Tags Remove(Tag tag)
    }

    Tags ..|> ITags
    Tags o-- Tag

    %% Validity Period
    class ValidityStart {
        <<Value Object>>
        +DateTimeOffset Value
    }

    class ValidityEnd {
        <<Value Object>>
        +DateTimeOffset Value
    }

    class ValidityPeriod {
        <<Value Object>>
        +bool IsActiveAt(DateTimeOffset moment)
        +bool IsExpiredAt(DateTimeOffset moment)
    }

    class IValidityPeriod {
        <<Interface>>
        +bool IsActiveAt(DateTimeOffset moment)
        +bool IsExpiredAt(DateTimeOffset moment)
    }

    ValidityPeriod ..|> IValidityPeriod
    ValidityPeriod *-- ValidityStart
    ValidityPeriod *-- ValidityEnd

    %% Weight
    class BasisPoints {
        <<Value Object>>
        +ushort Value
    }

    class Percent {
        <<Value Object>>
        +double Value
    }

    class Weight {
        <<Value Object>>
    }

    class IBasisPoints {
        <<Interface>>
        +ushort Value
    }

    class IPercent {
        <<Interface>>
        +double Value
    }

    class IWeight {
        <<Interface>>
        +BasisPoints ToBasisPoints()
        +Percent ToPercent()
    }

    BasisPoints ..|> IBasisPoints
    Percent ..|> IPercent
    Weight ..|> IWeight
    Weight *-- BasisPoints
    Weight *-- Percent

    %% Rating Components
    class RatingScore {
        <<Value Object>>
        +ushort Value
    }

    class RatingLabel {
        <<Value Object>>
        +string Value
    }

    class RatingAnnotation {
        <<Value Object>>
        +string Value
    }

    class RatingOption {
        <<Value Object>>
        +bool Matches(RatingScore score)
    }

    class IRatingOption {
        <<Interface>>
        +bool Matches(RatingScore score)
    }

    RatingOption ..|> IRatingOption
    RatingOption *-- RatingScore
    RatingOption *-- RatingLabel
    RatingOption *-- RatingAnnotation

    class RatingOptions {
        <<Value Object>>
        +RatingOptions WithSelectedScore(RatingScore score)
    }

    class IRatingOptions {
        <<Interface>>
        +RatingOptions WithSelectedScore(RatingScore score)
    }

    RatingOptions ..|> IRatingOptions
    RatingOptions o-- IRatingOption

    %% Criterion Components
    class CriterionTitle {
        <<Value Object>>
        +string Value
    }

    class CriterionText {
        <<Value Object>>
        +string Value
    }

    class GroupTitle {
        <<Value Object>>
        +string Value
    }

    class GroupDescription {
        <<Value Object>>
        +string Value
    }

    class OrderIndex {
        <<Value Object>>
        +ushort Value
    }

    %% Enums
    class FormStatus {
        <<Enumeration>>
        Draft
        Published
        Archived
    }

    %% Common
    class Option~T~ {
        <<Value Object>>
        +Map~TR~(Func~T,TR~ map) Option~TR~
        +Bind~TR~(Func~T,Option~TR~~ bind) Option~TR~
        +Reduce(T orElse) T
    }

    %% Exceptions
    class ScoreNotFoundException {
        <<Exception>>
        +RatingScore Score
    }

    ScoreNotFoundException --> RatingScore
```
