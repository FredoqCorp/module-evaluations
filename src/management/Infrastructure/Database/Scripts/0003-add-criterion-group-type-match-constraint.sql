-- =====================================================
-- Migration: 0003 - Add Criterion-Group Type Match Constraint
-- Description: Ensures criteria can only be added to groups of matching type
-- Author: CascVel Team
-- Date: 2025-10-18
-- Compatible with: PostgreSQL 13+
-- Notes: Idempotent - safe to run multiple times
-- =====================================================

-- =====================================================
-- Function: Check criterion type matches group type
-- =====================================================
CREATE OR REPLACE FUNCTION check_criterion_group_type_match()
RETURNS TRIGGER AS $$
BEGIN
    -- If group_id is provided, verify type compatibility with group
    IF NEW.group_id IS NOT NULL THEN
        -- Check if a group with matching type exists
        IF NOT EXISTS (
            SELECT 1
            FROM form_groups
            WHERE id = NEW.group_id
            AND group_type = NEW.criterion_type
        ) THEN
            RAISE EXCEPTION
                'Criterion type "%" does not match parent group type. Criterion ID: %, Group ID: %',
                NEW.criterion_type, NEW.id, NEW.group_id
                USING HINT = 'Average criteria can only be added to average groups, and weighted criteria to weighted groups';
        END IF;
    -- If form_id is provided (and group_id is NULL), verify type compatibility with form
    ELSIF NEW.form_id IS NOT NULL THEN
        -- Check if a form with matching type exists
        IF NOT EXISTS (
            SELECT 1
            FROM evaluation_forms
            WHERE id = NEW.form_id
            AND form_type = NEW.criterion_type
        ) THEN
            RAISE EXCEPTION
                'Criterion type "%" does not match parent form type. Criterion ID: %, Form ID: %',
                NEW.criterion_type, NEW.id, NEW.form_id
                USING HINT = 'Average criteria can only be added to average forms, and weighted criteria to weighted forms';
        END IF;
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- =====================================================
-- Trigger: Validate criterion-group type match on insert/update
-- =====================================================
DO $$
BEGIN
    -- Drop trigger if it exists
    DROP TRIGGER IF EXISTS trg_check_criterion_group_type_match ON form_criteria;

    -- Create trigger
    CREATE TRIGGER trg_check_criterion_group_type_match
    BEFORE INSERT OR UPDATE ON form_criteria
    FOR EACH ROW
    EXECUTE FUNCTION check_criterion_group_type_match();
END $$;

COMMENT ON FUNCTION check_criterion_group_type_match() IS
'Validates that a criterion type (average/weighted) matches its parent group type';

COMMENT ON TRIGGER trg_check_criterion_group_type_match ON form_criteria IS
'Prevents adding average criteria to weighted groups and vice versa';
