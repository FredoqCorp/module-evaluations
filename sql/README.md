# Database scripts

This folder contains generated SQL scripts produced from EF Core migrations.

- `baseline.sql` – first migration script to create schema from scratch.
- Future scripts will follow pattern: `<yyyyMMddHHmmss>_<Name>.sql`.

Best practices:
- Scripts are generated from migrations in `src/Infrastructure`.
- Never hand-edit generated scripts; keep changes in code/migrations.
- Keep database-agnostic SQL where possible; this project targets PostgreSQL.
