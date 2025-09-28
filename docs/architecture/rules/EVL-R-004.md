# EVL-R-004 — Editing rules

Statement
- Editing a form is recorded and allowed only while the form is not archived

Area
- Form maintenance

Preconditions
- New content is provided
- The current state allows editing

Invariants
- Each edit is captured in the audit history with a timestamp

Outcome
- The form content reflects the provided changes and the audit history is extended

Violation Handling
- If the form is archived or the state otherwise forbids editing, the change is rejected
