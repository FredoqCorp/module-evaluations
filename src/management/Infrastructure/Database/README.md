# Database Migrations with DbUp

This directory contains database migration infrastructure using [DbUp](https://dbup.readthedocs.io/).

## Structure

```
Database/
├── README.md                    # This file
├── DatabaseMigrator.cs          # DbUp migration runner
└── Scripts/                     # Migration scripts (embedded resources)
    ├── 0001-initial-schema.sql
    └── 0002-add-type-constraints.sql
```

## Migration Scripts

- **Location**: `Scripts/*.sql`
- **Naming convention**: `{number}-{description}.sql` (e.g., `0001-initial-schema.sql`)
- **Order**: Scripts execute in alphabetical order by filename
- **Idempotency**: All scripts are idempotent using `CREATE IF NOT EXISTS`, `DO $$` blocks, etc.
- **Tracking**: DbUp automatically creates `schemaversions` table to track executed scripts

## DbUp Tracking Table

DbUp creates and maintains the `schemaversions` table:

```sql
CREATE TABLE schemaversions (
    schemaversionsid SERIAL PRIMARY KEY,
    scriptname VARCHAR(255) NOT NULL,
    applied TIMESTAMP NOT NULL
);
```

Each successfully executed script is recorded with its name and timestamp.

## Usage

### In Integration Tests

The `DatabaseFixture` automatically runs migrations when starting Testcontainers:

```csharp
public class DatabaseFixture : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        // DbUp automatically runs pending migrations
        var result = DatabaseMigrator.MigrateDatabase(ConnectionString);

        if (!result.Successful)
            throw new InvalidOperationException($"Migration failed: {result.Error}");
    }
}
```

### Programmatically

```csharp
using CascVel.Modules.Evaluations.Management.Infrastructure.Database;

// Run all pending migrations
var result = DatabaseMigrator.MigrateDatabase(connectionString);

if (result.Successful)
    Console.WriteLine("Migration successful!");
else
    Console.WriteLine($"Migration failed: {result.Error}");
```

### Check if Upgrade Needed

```csharp
bool needsMigration = DatabaseMigrator.IsUpgradeRequired(connectionString);

if (needsMigration)
{
    Console.WriteLine("Pending migrations:");
    var scripts = DatabaseMigrator.GetScriptsToExecute(connectionString);
    foreach (var script in scripts)
    {
        Console.WriteLine($"  - {script}");
    }
}
```

## Adding New Migrations

### Step 1: Create SQL Script

Create a new file in `Scripts/` directory:

```bash
touch Scripts/0003-add-new-feature.sql
```

**Important**: Use sequential numbering (0001, 0002, 0003...).

### Step 2: Write Idempotent SQL

```sql
-- =====================================================
-- Migration: 0003 - Add New Feature
-- Description: Brief description of changes
-- Author: Your Name
-- Date: 2025-10-11
-- Compatible with: PostgreSQL 13+
-- Notes: Idempotent - safe to run multiple times
-- =====================================================

-- Example: Add new column
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.columns
        WHERE table_name = 'forms'
        AND column_name = 'new_column'
    ) THEN
        ALTER TABLE forms ADD COLUMN new_column VARCHAR(100);
    END IF;
END $$;

COMMENT ON COLUMN forms.new_column IS 'Description of new column';
```

### Step 3: Build Project

SQL scripts are embedded as resources during build:

```bash
dotnet build
```

### Step 4: Run Tests

```bash
dotnet test tests/management/Infrastructure.IntegrationTests/
```

The new migration will execute automatically when tests run.

## Idempotency Patterns

### Tables

```sql
CREATE TABLE IF NOT EXISTS table_name (...);
```

### Indexes

```sql
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_indexes
        WHERE indexname = 'idx_name'
    ) THEN
        CREATE INDEX idx_name ON table_name (column);
    END IF;
END $$;
```

### Constraints

```sql
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'constraint_name'
    ) THEN
        ALTER TABLE table_name
        ADD CONSTRAINT constraint_name CHECK (...);
    END IF;
END $$;
```

### Foreign Keys

```sql
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'fk_name'
    ) THEN
        ALTER TABLE child_table
        ADD CONSTRAINT fk_name
        FOREIGN KEY (column)
        REFERENCES parent_table (id);
    END IF;
END $$;
```

## Production Deployment

For production deployments, a separate migrator application will be created. For now, migrations run automatically in:

1. **Integration tests** - via `DatabaseFixture`
2. **Development** - programmatically via `DatabaseMigrator.MigrateDatabase()`

Future: Standalone migrator CLI tool for production use.

## Rollbacks

DbUp does not support automatic rollbacks. If you need to revert changes:

1. Create a new forward migration that reverses the changes
2. Number it sequentially (e.g., `0004-revert-feature.sql`)
3. Use `DROP ... IF EXISTS` for idempotency

Example:

```sql
-- 0004-revert-new-column.sql
DO $$
BEGIN
    IF EXISTS (
        SELECT 1 FROM information_schema.columns
        WHERE table_name = 'forms'
        AND column_name = 'new_column'
    ) THEN
        ALTER TABLE forms DROP COLUMN new_column;
    END IF;
END $$;
```

## Troubleshooting

### Script not found

**Error**: "No scripts need to be executed"

**Solution**: Ensure script is:
1. Located in `Database/Scripts/` folder
2. Named with `.sql` extension
3. Project rebuilt (`dotnet build`)

### Script executed but not applied

**Error**: Changes not visible in database

**Check**:
```sql
-- View executed scripts
SELECT * FROM schemaversions ORDER BY applied DESC;

-- Check if script is in journal
SELECT * FROM schemaversions WHERE scriptname LIKE '%0003%';
```

### Migration failed mid-execution

DbUp uses transactions per script. If a script fails:

1. Fix the script
2. Manually remove failed entry from `schemaversions` (if needed)
3. Re-run migration

```sql
-- Remove failed migration (if needed)
DELETE FROM schemaversions WHERE scriptname = 'failed-script.sql';
```

## Best Practices

✅ **DO**:
- Use sequential numbering (0001, 0002, 0003...)
- Make scripts idempotent
- Test locally before committing
- Add comments explaining complex changes
- Use transactions where appropriate

❌ **DON'T**:
- Modify existing scripts after they've been applied
- Skip version numbers
- Use database-specific features without checking compatibility
- Forget to handle idempotency

## References

- [DbUp Documentation](https://dbup.readthedocs.io/)
- [PostgreSQL DO Statement](https://www.postgresql.org/docs/current/sql-do.html)
- [PostgreSQL System Catalogs](https://www.postgresql.org/docs/current/catalogs.html)
