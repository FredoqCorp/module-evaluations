# EVL-R-003 — Starting an evaluation requires a compatible scoring method

Statement
- Starting an evaluation session is allowed only if the chosen scoring method is compatible with the current version of the form

Area
- Evaluation sessions

Preconditions
- A scoring method is selected

Invariants
- Compatibility is checked before the session starts; when incompatible, the session must not start

Outcome
- An evaluation session starts with the form structure and the chosen scoring method locked for the duration of the session

Violation Handling
- When incompatible, the system rejects starting the session
