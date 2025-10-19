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
DECLARE
    parent_form uuid;
    parent_group_type varchar(20);
    form_type varchar(20);
BEGIN
    IF NEW.group_id IS NOT NULL THEN
        SELECT form_id, group_type
        INTO parent_form, parent_group_type
        FROM form_groups
        WHERE id = NEW.group_id;

        IF NOT FOUND THEN
            RAISE EXCEPTION
                'Parent group "%" was not found. Criterion ID: %',
                NEW.group_id, NEW.id
                USING HINT = 'Persist the parent group before referencing it from form_criteria';
        END IF;

        IF parent_group_type <> NEW.criterion_type THEN
            RAISE EXCEPTION
                'Criterion type "%" does not match parent group type "%". Criterion ID: %, Group ID: %',
                NEW.criterion_type, parent_group_type, NEW.id, NEW.group_id
                USING HINT = 'Average criteria can only be added to average groups, and weighted criteria to weighted groups';
        END IF;

        IF NEW.form_id IS NULL THEN
            NEW.form_id := parent_form;
        ELSIF NEW.form_id <> parent_form THEN
            RAISE EXCEPTION
                'Criterion references inconsistent form "%". Expected form "%". Criterion ID: %',
                NEW.form_id, parent_form, NEW.id
                USING HINT = 'Persist criteria with the same form as their parent group';
        END IF;

        SELECT root_group_type
        INTO form_type
        FROM forms
        WHERE id = parent_form;

        IF NOT FOUND THEN
            RAISE EXCEPTION
                'Parent form "%" was not found for group "%". Criterion ID: %',
                parent_form, NEW.group_id, NEW.id
                USING HINT = 'Persist the form before its groups and criteria';
        END IF;

        IF form_type <> NEW.criterion_type THEN
            RAISE EXCEPTION
                'Criterion type "%" does not match parent form type "%". Criterion ID: %, Form ID: %',
                NEW.criterion_type, form_type, NEW.id, parent_form
                USING HINT = 'Average criteria can only be added to average forms, and weighted criteria to weighted forms';
        END IF;
    ELSIF NEW.form_id IS NOT NULL THEN
        SELECT root_group_type
        INTO form_type
        FROM forms
        WHERE id = NEW.form_id;

        IF NOT FOUND THEN
            RAISE EXCEPTION
                'Parent form "%" was not found. Criterion ID: %',
                NEW.form_id, NEW.id
                USING HINT = 'Persist the form before its criteria';
        END IF;

        IF form_type <> NEW.criterion_type THEN
            RAISE EXCEPTION
                'Criterion type "%" does not match parent form type "%". Criterion ID: %, Form ID: %',
                NEW.criterion_type, form_type, NEW.id, NEW.form_id
                USING HINT = 'Average criteria can only be added to average forms, and weighted criteria to weighted forms';
        END IF;
    ELSE
        RAISE EXCEPTION
            'Criterion "%" must reference either a form or a group',
            NEW.id
            USING HINT = 'Populate form_id for standalone criteria or group_id for grouped criteria';
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
