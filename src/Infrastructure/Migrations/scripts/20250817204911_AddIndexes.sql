START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM evaluations."__EFMigrationsHistory" WHERE "MigrationId" = '20250817204911_AddIndexes') THEN
    CREATE INDEX ix_evalform_status ON evaluations.evaluation_forms (status);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM evaluations."__EFMigrationsHistory" WHERE "MigrationId" = '20250817204911_AddIndexes') THEN
    CREATE UNIQUE INDEX ux_evaluation_forms_code ON evaluations.evaluation_forms (code);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM evaluations."__EFMigrationsHistory" WHERE "MigrationId" = '20250817204911_AddIndexes') THEN
    CREATE INDEX ix_form_groups_order ON evaluations.form_groups (order_index);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM evaluations."__EFMigrationsHistory" WHERE "MigrationId" = '20250817204911_AddIndexes') THEN
    CREATE INDEX ix_form_criteria_order ON evaluations.form_criteria (order_index);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM evaluations."__EFMigrationsHistory" WHERE "MigrationId" = '20250817204911_AddIndexes') THEN
    INSERT INTO evaluations."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20250817204911_AddIndexes', '9.0.8');
    END IF;
END $EF$;
COMMIT;

