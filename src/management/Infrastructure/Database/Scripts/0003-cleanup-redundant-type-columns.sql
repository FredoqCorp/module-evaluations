-- =====================================================
-- Migration: 0003 - Cleanup Redundant Type Columns
-- Description: Removes legacy group and criterion discriminators plus related trigger
-- Author: CascVel Team
-- Date: 2025-10-18
-- Compatible with: PostgreSQL 13+
-- Notes: Idempotent - safe to run multiple times
-- =====================================================

-- =====================================================
-- Step 1: Drop trigger enforcing legacy type checks
-- =====================================================
DO $$
BEGIN
    IF EXISTS (
        SELECT 1
        FROM pg_trigger
        WHERE tgname = 'trg_check_criterion_group_type_match'
          AND tgrelid = 'form_criteria'::regclass
    ) THEN
        DROP TRIGGER trg_check_criterion_group_type_match ON form_criteria;
    END IF;
END $$;

-- =====================================================
-- Step 2: Drop supporting function if defined
-- =====================================================
DO $$
BEGIN
    IF EXISTS (
        SELECT 1
        FROM pg_proc
        WHERE proname = 'check_criterion_group_type_match'
          AND pg_function_is_visible(oid)
    ) THEN
        DROP FUNCTION check_criterion_group_type_match() CASCADE;
    END IF;
END $$;

-- =====================================================
-- Step 3: Drop redundant discriminator columns
-- =====================================================
DO $$
BEGIN
    IF EXISTS (
        SELECT 1
        FROM information_schema.columns
        WHERE table_name = 'form_groups'
          AND column_name = 'group_type'
    ) THEN
        ALTER TABLE form_groups
        DROP COLUMN group_type;
    END IF;
END $$;

DO $$
BEGIN
    IF EXISTS (
        SELECT 1
        FROM information_schema.columns
        WHERE table_name = 'form_criteria'
          AND column_name = 'criterion_type'
    ) THEN
        ALTER TABLE form_criteria
        DROP COLUMN criterion_type;
    END IF;
END $$;
