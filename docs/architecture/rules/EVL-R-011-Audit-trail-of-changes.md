# EVL-R-011 — Audit trail of changes

Statement
- The system records who made a change, when it was made, and what was changed for all lifecycle and content changes

Area
- Audit and compliance

Preconditions
- A create, edit, publish, or archive action is performed

Invariants
- Each change captures actor identity, timestamp, and change type; details of the change are recorded at an appropriate level for audit

Outcome
- A complete, inspectable history of changes exists for each form

Violation Handling
- If the audit record cannot be created, the change is rejected

