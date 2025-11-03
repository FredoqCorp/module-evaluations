# ADR: Root Group Type Drives Calculation Strategy

Status
- Accepted

Context
- `forms.root_group_type` already captures the calculation strategy for an entire evaluation form
- Persisting duplicate discriminators (`group_type`, `criterion_type`) across all nested entities introduced redundancy
- Application services always derive group and criterion behavior from the form root configuration
- Triggers enforcing cross-entity type consistency coupled strong dependencies to database internals

Decision
- Remove `group_type` and `criterion_type` columns from `form_groups` and `form_criteria`
- Derive calculation behavior exclusively from `forms.root_group_type`
- Simplify migration `0003` to drop legacy columns, triggers, and supporting functions
- Rely on application-layer validation to ensure weight-related invariants per calculation strategy

Consequences
- Database schema eliminates redundant discriminator data and related trigger maintenance
- Insert and update operations require fewer parameters, reducing risk of inconsistent payloads
- Application services remain responsible for enforcing weighting rules using a single source of truth
- Historical fixtures referencing `group_type` or `criterion_type` must be updated
- Future calculation modes extend the enumeration only at the form level

References
- Database schema overview: `docs/architecture/database-schema.md`
- Migration scripts: `src/management/Infrastructure/Database/Scripts/0003-cleanup-redundant-type-columns.sql`
