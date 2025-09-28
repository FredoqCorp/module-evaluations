# EVL-R-008 — Form structure and grouping

Statement
- A form designer can organize criteria into groups, including nesting groups inside other groups; criteria may also exist at the root of a form without a group

Area
- Form structure

Preconditions
- A form is being designed or edited

Invariants
- Groups may contain criteria and/or subgroups
- Criteria may exist either at the root or inside any group

Outcome
- The system stores a hierarchical structure of groups and criteria

Violation Handling
- If the structure is malformed, the change is rejected

