# EVL-R-014: Criterion Rating Scales

**Status**: Draft
**Created**: 2025-09-30
**Related**: EVL-R-013, EVL-R-010

## Business Context

When authoring evaluation forms, designers need to specify how each criterion will be rated. Rating scales define the set of possible scores that evaluators can assign, along with human-readable labels and optional descriptions. WebArm renders the scale selector and translates the designer choice into individual rating options that the backend stores.

## Requirements

### Scale Types

The system must support two types of rating scales, selected on the front-end:

1. **Binary Scale**: Two-point scale for yes/no decisions
   - Fixed numeric values: 1 (No) and 5 (Yes)
   - Used for criteria that require simple presence/absence evaluation

2. **Five-Point Scale**: Five-point Likert-like scale for nuanced evaluation
   - Fixed numeric values: 1, 2, 3, 4, 5
   - Used for criteria requiring gradual assessment

### Scale Extensibility

- The domain model must support adding new scale types in the future without modifying existing code
- Each scale type has a fixed set of numeric values that will be used in score calculations (see EVL-R-010)
- Backend validation works over the submitted rating options and ensures that the numeric set matches one of the supported scales.

### Rating Labels and Annotations

For each numeric value in a scale, designers must provide:

1. **Label** (Required): Short text identifying the rating option
   - Must be provided by the designer
   - No uniqueness constraint within the scale
   - No default values provided by the system
   - Examples: "Strongly Disagree", "Not Met", "Excellent"

2. **Annotation** (Optional): Detailed description explaining the rating option
   - Helps evaluators understand when to select this option
   - Examples: "The candidate demonstrates no understanding of the topic", "All requirements are fully satisfied"

### Localization

- Current implementation does not require multi-language support
- Labels and annotations are stored in a single language

### Scale Modification

- Designers can change the scale type and all labels/annotations freely before form publication
- When changing scale type, the front-end regenerates the rating options and discards the previous configuration
- After form publication, scale modifications follow the form versioning rules (EVL-R-004)

### Separation of Concerns

- Rating scale configuration is independent of form validity periods
- Scale changes do not automatically trigger form versioning
- ValidityPeriod applies to the entire form, not individual scale configurations
- The front-end owns the logic of mapping a scale type to the correct set of rating options submitted to the backend.

## Domain Rules

1. Each criterion must supply rating options that match the selected scale
2. Rating options must have a fixed set of numeric values based on the scale type
3. Binary scale options always use values: 1, 5
4. Five-point scale options always use values: 1, 2, 3, 4, 5
5. Each numeric value must have a label (non-empty string)
6. Each numeric value may have an annotation (optional string)
7. Labels within a scale do not need to be unique
8. Scale type can be changed before publication without versioning
9. Numeric values cannot be customized by designers

## Examples

### Binary Scale Example
```
Type: Binary
Values:
  1: Label="Not Met", Annotation="The requirement is not satisfied"
  5: Label="Met", Annotation="The requirement is fully satisfied"
```

### Five-Point Scale Example
```
Type: Five-Point
Values:
  1: Label="Poor", Annotation="Does not meet minimum expectations"
  2: Label="Below Average", Annotation=null
  3: Label="Average", Annotation=null
  4: Label="Good", Annotation="Exceeds expectations in some areas"
  5: Label="Excellent", Annotation="Exceeds expectations in all areas"
```

## Future Considerations

- Ten-point scales
- Custom numeric ranges
- Multi-language label/annotation support
- Scale templates and reuse across forms
- Visual scale representations (emoticons, stars, etc.)
