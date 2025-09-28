# EVL-R-014 — Stable ordering of groups and criteria

Statement
- The designer must see groups and criteria in the same order as they were saved

Area
- User experience and ordering

Preconditions
- A form with groups and/or criteria has been saved

Invariants
- Display and subsequent operations use the stored order
- Reordering is an explicit operation; otherwise the order remains unchanged

Outcome
- The system preserves and presents the saved order of groups and criteria

Violation Handling
- If the order cannot be determined due to inconsistent data, saving or presenting the structure is rejected until corrected

