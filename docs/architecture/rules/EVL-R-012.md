# EVL-R-012 — Form validity period

Statement
- A form may have a validity period that defines when it can be considered applicable

Area
- Availability and scheduling

Preconditions
- A start and end moment may be provided; the period may be open-ended

Invariants
- When both start and end are provided, the end is not earlier than the start

Outcome
- The system stores the validity period and uses it to determine the form's availability according to platform policies

Violation Handling
- If the period is inconsistent, the change is rejected

