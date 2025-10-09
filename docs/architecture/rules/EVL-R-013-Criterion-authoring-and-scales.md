# EVL-R-013 — Criterion authoring and scales

Statement
- A form designer can add a criterion by providing criterion text, a criterion title, and a rating scale; the system supports Five-point and Binary (Yes/No) scales

Area
- Authoring

Preconditions
- Title, text, and a scale are provided

Invariants
- Title length is at most 100 characters and is not empty
- Text length is at most 1000 characters and is not empty
- Scale is one of the supported scales: Five-point or Binary (Yes/No)

Outcome
- The criterion is stored with the specified title, text, and scale and can be used in evaluations

Violation Handling
- If any requirement is not satisfied, creating or updating the criterion is rejected

