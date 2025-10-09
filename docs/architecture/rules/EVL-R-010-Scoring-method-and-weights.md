# EVL-R-010 — Scoring method and weights

Statement
- A form designer selects a scoring method; the system supports Average and Weighted Average; for the Weighted Average method, weights can be assigned to groups and criteria, and on publication the sum of sibling weights at each level must equal 100

Area
- Scoring and publication validation

Preconditions
- A scoring method is selected; for Weighted Average, weights are provided for all items that participate in weighting

Invariants
- For Weighted Average, the sum of weights for sibling criteria inside any group equals 100
- For Weighted Average, the sum of weights for sibling subgroups inside any group equals 100
- For Weighted Average, where criteria and subgroups coexist at the same level, the applicable policy must define how they are combined; if combined, their total must equal 100
- For Weighted Average, the synthetic root group is treated as a regular group, so root-level criteria and groups must also sum to 100

Outcome
- Only forms with a valid weighting scheme can be published under the Weighted Average method; Average requires no weights

Violation Handling
- If required weights are missing or sums do not equal 100 at any level, publication is rejected
