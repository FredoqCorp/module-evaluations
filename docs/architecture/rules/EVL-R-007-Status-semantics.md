# EVL-R-007 — Status semantics

Statement
- The system displays a form status based on the latest lifecycle event: Draft, Published, or Archived

Area
- Visibility and communication

Preconditions
- There is at least one recorded lifecycle event

Invariants
- Status reflects the last event consistently across the system

Outcome
- Users see the current status of the form and can act according to policies defined for that status

Violation Handling
- If the status cannot be determined, the operation is rejected until the inconsistency is resolved

