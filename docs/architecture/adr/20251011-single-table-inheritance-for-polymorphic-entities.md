# ADR: Single Table Inheritance for Polymorphic Domain Entities

Status
- Accepted

Context
- The domain model contains polymorphic entities (groups and criteria) with two type variants: Average and Weighted
- Type variants differ minimally - only by presence of weight_basis_points field
- Groups form hierarchical tree structures with parent-child relationships
- Forms are loaded as complete aggregates requiring all related data
- Future extensions may introduce additional type variants with unique characteristics

Decision
- Use Single Table Inheritance (STI) pattern for `form_groups` and `form_criteria` tables
- Store type discriminator in `group_type` and `criterion_type` columns
- Enforce type-specific constraints through CHECK constraints at database level
- Use nullable columns for type-specific fields (e.g., `weight_basis_points`)
- For future types with many unique fields, use JSONB columns instead of adding multiple nullable columns

Scope
- Infrastructure layer database schema design
- Applies to `form_groups`, `form_criteria`, and related polymorphic entities
- Does not affect domain model architecture (interfaces and implementations remain unchanged)

Rationale
- **Minimal type differences**: Average and Weighted variants differ only by one field, making separate tables unnecessary
- **Hierarchical queries**: Self-referencing parent_id relationships are simpler with single table
- **Unified ordering**: order_index must work across types within same context
- **Performance**: Loading complete form aggregates requires fewer JOINs
- **Simplicity**: Queries, migrations, and repository code remain straightforward
- **Type safety**: CHECK constraints enforce business rules at database level

Consequences
- Nullable columns required for type-specific fields
- CHECK constraints must be updated when adding new type variants
- Database-level validation prevents invalid type-field combinations
- Simpler repository implementations with fewer queries
- Adding new types requires migration to update CHECK constraints
- Better performance for aggregate loading compared to Table Per Type approach

Examples
```sql
-- Groups table with discriminator
CREATE TABLE form_groups (
    id uuid PRIMARY KEY,
    group_type varchar(20) NOT NULL,
    weight_basis_points int NULL,
    -- ... other fields
    CONSTRAINT chk_weight_by_type CHECK (
        (group_type = 'weighted' AND weight_basis_points IS NOT NULL) OR
        (group_type = 'average' AND weight_basis_points IS NULL)
    )
);

-- Adding new type variant (future extension)
ALTER TABLE form_groups
ADD CONSTRAINT chk_valid_type
CHECK (group_type IN ('average', 'weighted', 'hybrid'));
```

Validation
- CHECK constraints prevent invalid data at insert/update time
- Integration tests verify constraints work correctly
- Migration tests ensure up/down migrations maintain data integrity

Alternatives Considered
- **Table Per Type (TPT)**: Separate tables for Average and Weighted variants — rejected due to complexity in hierarchical queries, difficulty maintaining order across types, and performance overhead from UNION operations
- **Table Per Hierarchy with JSONB for all differences**: Store all type-specific data in JSONB column — rejected for initial implementation as single nullable column is simpler; remains option for future complex types
- **Class Table Inheritance**: Base table with type-specific child tables — rejected due to foreign key complexity and JOIN overhead for aggregate loading

References
- Martin Fowler - Patterns of Enterprise Application Architecture (Single Table Inheritance pattern)
- Database schema: `src/management/Infrastructure/Migrations/202510100001_InitialSchema.cs`
- Type constraints: `src/management/Infrastructure/Migrations/202510100002_AddTypeConstraints.cs`
