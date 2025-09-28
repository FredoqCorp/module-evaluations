# EVL-R-001 — Form meta fields

Statement
- A form must have a name, may have a description, may have a list of keywords, and must have a code

Area
- Form description and cataloging

Preconditions
- Name is provided
- Code is provided
- Keywords list may be empty

Invariants
- Description is optional
- Keywords are case-insensitive and unique, each item is non-empty
- Code is unique across all forms
- Name length is at most 100 characters
- When provided, description length is at most 1000 characters

Outcome
- The system stores form metadata consisting of name, description, keywords, and code

Violation Handling
- If any requirement is not satisfied, the operation to create or update the form is rejected

