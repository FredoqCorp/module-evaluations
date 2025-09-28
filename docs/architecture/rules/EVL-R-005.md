# EVL-R-005 — Publishing constraints

Statement
- Publishing a form makes it available for use and restricts further changes according to policy

Area
- Publication

Preconditions
- A decision to publish is made and the publication time is known

Invariants
- After publication, only allowed fields may change (if any); content changes that affect ongoing evaluations are forbidden

Outcome
- The form becomes available for creating new evaluation sessions

Violation Handling
- Attempts to perform forbidden changes after publication are rejected
