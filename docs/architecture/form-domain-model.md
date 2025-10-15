# Form Domain Model

## Aggregate Overview

```mermaid
classDiagram
    class Form {
        <<Aggregate Root>>
        +void Validate()
    }

    class FormId {
        <<Value Object>>
        +Guid Value
    }

    class FormMetadata {
        <<Value Object>>
        +FormName Name()
        +FormDescription Description()
        +FormCode Code()
        +ITags Tags()
    }

    class FormName {
        <<Value Object>>
        +string Value
    }

    class FormDescription {
        <<Value Object>>
        +string Value
    }

    class FormCode {
        <<Value Object>>
        +string Token
    }

    class ITags {
        <<Interface>>
        +ITags With(Tag tag)
    }

    class Tag {
        <<Value Object>>
        +string Text
    }

    class IFormRootGroup {
        <<Interface>>
        +IRatingContribution Contribution()
        +void Validate()
    }

    Form *-- FormId
    Form *-- FormMetadata
    Form *-- IFormRootGroup
    FormMetadata o-- ITags
    ITags o-- Tag
```

The form now keeps a single structural root (`IFormRootGroup`). All other criteria and groups hang off this node, which simplifies validation and isolates distinct scoring strategies.

## Общие интерфейсы

```mermaid
classDiagram
    class IRatingContribution {
        <<Interface>>
        +IRatingContribution Join(IRatingContribution contribution)
        +Option~decimal~ Total()
        +T Accept~T~(Func~decimal, ushort, T~ projector)
    }

    class IRatingContributionSource {
        <<Interface>>
        +IRatingContribution Contribution()
    }

    class ICriterion {
        <<Interface>>
        +IRatingContribution Contribution()
        +void Validate()
    }

    class IGroup {
        <<Interface>>
        +IRatingContribution Contribution()
        +void Validate()
    }

    class ICriteria {
        <<Interface>>
        +IRatingContribution Contribution()
        +void Validate()
    }

    class IGroups {
        <<Interface>>
        +IRatingContribution Contribution()
        +void Validate()
    }

    ICriterion ..|> IRatingContributionSource
    ICriteria ..|> IRatingContributionSource
    IGroup ..|> IRatingContributionSource
    IGroups ..|> IRatingContributionSource
```

These interfaces remain the foundational contracts for both the Average and Weighted branches.

## Average Policy

```mermaid
classDiagram
    class IAverageCriterion {
        <<Interface>>
    }

    class IAverageCriteria {
        <<Interface>>
    }

    class IAverageGroup {
        <<Interface>>
    }

    class IAverageGroups {
        <<Interface>>
    }

    class AverageRootGroup {
        <<Entity>>
        +IRatingContribution Contribution()
        +void Validate()
    }

    class AverageCriterionGroup {
        <<Entity>>
        +IRatingContribution Contribution()
        +void Validate()
    }

    class AverageCriteria {
        <<Entity>>
        +IRatingContribution Contribution()
        +void Validate()
    }

    class AverageGroups {
        <<Entity>>
        +IRatingContribution Contribution()
        +void Validate()
    }

    class Criterion {
        <<Entity>>
        +IRatingContribution Contribution()
        +void Validate()
    }

    class IRatingOptions {
        <<Interface>>
        +IRatingContribution Contribution()
    }

    class GroupProfile {
        <<Value Object>>
        +GroupId Id
        +GroupTitle Title
        +GroupDescription Description
    }

    class GroupId {
        <<Value Object>>
        +Guid Value
    }

    class GroupTitle {
        <<Value Object>>
        +string Text
    }

    class GroupDescription {
        <<Value Object>>
        +string Text
    }

    class CriterionId {
        <<Value Object>>
        +Guid Value
    }

    class CriterionTitle {
        <<Value Object>>
        +string Text
    }

    class CriterionText {
        <<Value Object>>
        +string Text
    }

    IAverageCriterion ..|> ICriterion
    IAverageCriteria ..|> ICriteria
    IAverageGroup ..|> IGroup
    IAverageGroups ..|> IGroups
    AverageRootGroup ..|> IFormRootGroup
    AverageRootGroup ..|> IAverageGroup
    AverageCriterionGroup ..|> IAverageGroup
    AverageCriteria ..|> IAverageCriteria
    AverageGroups ..|> IAverageGroups
    Criterion ..|> IAverageCriterion

    AverageRootGroup o-- IAverageCriteria
    AverageRootGroup o-- IAverageGroups
    AverageGroups o-- IAverageGroup
    AverageCriterionGroup o-- GroupProfile
    AverageCriterionGroup o-- IAverageCriteria
    AverageCriterionGroup o-- IAverageGroups
    AverageCriteria o-- IAverageCriterion
    Criterion *-- CriterionId
    Criterion *-- CriterionTitle
    Criterion *-- CriterionText
    Criterion *-- IRatingOptions
    GroupProfile *-- GroupId
    GroupProfile *-- GroupTitle
    GroupProfile *-- GroupDescription
```

The Average branch treats `GroupProfile` as the single bundle of identity and descriptive data. The root (`AverageRootGroup`) accepts only `IAverageCriteria` and `IAverageGroups`, preventing weighted elements from being injected.

## Weighted Average Policy

```mermaid
classDiagram
    class IWeightedCriterion {
        <<Interface>>
        +IWeight Weight()
    }

    class IWeightedCriteria {
        <<Interface>>
        +IBasisPoints Weight()
    }

    class IWeightedGroup {
        <<Interface>>
        +IWeight Weight()
    }

    class IWeightedGroups {
        <<Interface>>
        +IBasisPoints Weight()
    }

    class WeightedRootGroup {
        <<Entity>>
        +IRatingContribution Contribution()
        +void Validate()
    }

    class WeightedCriterionGroup {
        <<Entity>>
        +IRatingContribution Contribution()
        +void Validate()
        +IWeight Weight()
    }

    class WeightedCriteria {
        <<Entity>>
        +IRatingContribution Contribution()
        +IBasisPoints Weight()
        +void Validate()
    }

    class WeightedGroups {
        <<Entity>>
        +IRatingContribution Contribution()
        +IBasisPoints Weight()
        +void Validate()
    }

    class WeightedCriterion {
        <<Entity>>
        +IRatingContribution Contribution()
        +IWeight Weight()
        +void Validate()
    }

    class IBasisPoints {
        <<Interface>>
        +IPercent Percent()
        +decimal Apply(decimal value)
    }

    class IPercent {
        <<Interface>>
        +IBasisPoints Basis()
    }

    class IWeight {
        <<Interface>>
        +IPercent Percent()
        +IRatingContribution Weighted(IRatingContribution contribution)
    }

    class BasisPoints {
        <<Value Object>>
        +IPercent Percent()
        +decimal Apply(decimal value)
    }

    class Percent {
        <<Value Object>>
        +IBasisPoints Basis()
    }

    class Weight {
        <<Value Object>>
        +IPercent Percent()
        +IRatingContribution Weighted(IRatingContribution contribution)
    }

    IWeightedCriterion ..|> ICriterion
    IWeightedCriteria ..|> ICriteria
    IWeightedGroup ..|> IGroup
    IWeightedGroups ..|> IGroups
    WeightedRootGroup ..|> IFormRootGroup
    WeightedCriterionGroup ..|> IWeightedGroup
    WeightedCriteria ..|> IWeightedCriteria
    WeightedGroups ..|> IWeightedGroups
    WeightedCriterion ..|> IWeightedCriterion
    Weight ..|> IWeight
    Weight *-- IBasisPoints
    Weight --> IPercent
    BasisPoints ..|> IBasisPoints
    Percent ..|> IPercent
    BasisPoints --> Percent
    Percent --> BasisPoints
    WeightedCriterionGroup o-- GroupProfile
    WeightedCriterionGroup o-- IWeightedCriteria
    WeightedCriterionGroup o-- IWeightedGroups
    WeightedCriterionGroup *-- IWeight
    WeightedCriteria o-- IWeightedCriterion
    WeightedGroups o-- IWeightedGroup
    WeightedRootGroup o-- IWeightedCriteria
    WeightedRootGroup o-- IWeightedGroups
```

`WeightedRootGroup` works exclusively with weighted collections and enforces the root-level weight totals. Weight calculation and validation are shared between the collections (`IWeightedCriteria`, `IWeightedGroups`) and the concrete groups (`IWeightedGroup`).

## Invariants

- EVL-R-008: every criterion and subgroup resides inside the synthetic root group, enabling explicit structural validation before publication.
- EVL-R-010: for Weighted Average, sibling weights must add up to 100 % at every level, including the root, otherwise validation fails fast.
- Root objects prevent mixing implementations: `AverageRootGroup` accepts only `IAverage*`, while `WeightedRootGroup` accepts only `IWeighted*`.

These changes encode the scoring strategies in the type system and make it straightforward to wire up forms with the appropriate root object for the selected rule.

## Identity and Authorization

The module includes identity value objects and interfaces to support user context and role-based authorization:

```mermaid
classDiagram
    class UserId {
        <<Value Object>>
        +string Value
    }

    class UserInfo {
        <<Value Object>>
        +void Print(IMedia media)
    }

    class IUserInfo {
        <<Interface>>
        +void Print(IMedia media)
    }

    class IModuleUser {
        <<Interface>>
        +bool IsInRole(ModuleRole role)
        +IUserInfo GetUserInfo()
        +void Print(IMedia media)
    }

    class ModuleRole {
        <<Enumeration>>
        FormDesigner
        Supervisor
        Operator
    }

    UserInfo ..|> IUserInfo
    UserInfo *-- UserId
    UserInfo o-- "Option~string~" : username
    UserInfo o-- "Option~string~" : name
    UserInfo o-- "Option~string~" : email
    IModuleUser ..> ModuleRole : uses
    IModuleUser ..> IUserInfo : returns
```

### Value Objects

- **UserId**: String-based user identifier extracted from JWT token `sub` claim. Uses string to support various identity provider formats (GUIDs, numeric IDs, custom identifiers).

- **UserInfo**: Immutable record containing user identification and optional metadata:
  - Required: `UserId` - unique user identifier
  - Optional: `username` - login/username from `preferred_username` or `username` JWT claim
  - Optional: `name` - display name from `name` JWT claim or composed from `given_name` + `family_name`
  - Optional: `email` - email address from `email` JWT claim

  Uses `Option<string>` monad for optional fields to explicitly model absence of data. Follows Printer Pattern with `Print(IMedia)` method for serialization.

### Interfaces

- **IUserInfo**: Behavioral contract for user information value objects. Provides `Print(IMedia)` method for serialization.

- **IModuleUser**: Behavioral contract for accessing authenticated user information and checking role membership:
  - `IsInRole(ModuleRole)` - checks if user has specific module role
  - `GetUserInfo()` - returns user information as `IUserInfo`
  - `Print(IMedia)` - delegates to underlying `UserInfo.Print()`

  Implemented by infrastructure layer through `HttpContextModuleUser` which extracts claims from JWT tokens.

### Roles

Three distinct roles aligned with business domain:

- **FormDesigner**: Creates, edits, and manages evaluation forms
- **Supervisor**: Evaluates operator performance using forms
- **Operator**: Contact center agent being evaluated

### Media Abstraction

The `IMedia` interface provides methods for serializing domain objects:

- `WriteString(key, value)` - writes required string field
- `WriteOptionalString(key, Option<string>)` - writes optional string field only if present
- `WriteGuid(key, value)` - writes GUID field
- `WriteInt32(key, value)` - writes integer field
- `WriteStringArray(key, values)` - writes string array

This abstraction enables clean serialization without coupling domain objects to specific formats (JSON, XML, etc.).

See [ADR: JWT-Based Authorization](adr/20251014-jwt-based-authorization-with-module-roles.md) for authorization architecture details.
