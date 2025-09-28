# EVL-R-002 — Form lifecycle transitions

Statement
- Form state changes must follow allowed transitions and each next change must have a timestamp not earlier than the previous one

Area
- Form publication and archival

States And Transitions
- Draft → Draft | Published | Archived
- Published → Archived
- Archived → (no further transitions)

Preconditions
- A current state and a previous change timestamp are known

Invariants
- The next timestamp is greater than or equal to the previous timestamp

Outcome
- The system records the new state with its timestamp when a transition is allowed

Violation Handling
- If a transition is not allowed or timestamp order is broken, the change is rejected
