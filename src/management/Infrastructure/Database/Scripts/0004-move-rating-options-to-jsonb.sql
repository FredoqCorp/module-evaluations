-- =====================================================
-- Migration: 0004 - Move Rating Options to JSONB
-- Description: Refactors rating_options from separate table to JSONB column in form_criteria
-- Author: CascVel Team
-- Date: 2025-10-18
-- Compatible with: PostgreSQL 13+
-- Notes: Idempotent - safe to run multiple times
-- =====================================================

-- =====================================================
-- Step 1: Add rating_options column to form_criteria
-- =====================================================
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.columns
        WHERE table_name = 'form_criteria'
        AND column_name = 'rating_options'
    ) THEN
        ALTER TABLE form_criteria
        ADD COLUMN rating_options jsonb NOT NULL DEFAULT '[]'::jsonb;
    END IF;
END $$;

COMMENT ON COLUMN form_criteria.rating_options IS 'JSONB array of rating options with score, label, and annotation';

-- =====================================================
-- Step 2: Migrate existing data from rating_options table
-- =====================================================
DO $$
BEGIN
    -- Only migrate if rating_options table exists and has data
    IF EXISTS (
        SELECT 1 FROM information_schema.tables
        WHERE table_name = 'rating_options'
    ) THEN
        -- Migrate data
        UPDATE form_criteria fc
        SET rating_options = (
            SELECT jsonb_agg(
                jsonb_build_object(
                    'score', ro.score,
                    'label', ro.label,
                    'annotation', COALESCE(ro.annotation, '')
                ) ORDER BY ro.order_index
            )
            FROM rating_options ro
            WHERE ro.criterion_id = fc.id
        )
        WHERE EXISTS (
            SELECT 1 FROM rating_options ro
            WHERE ro.criterion_id = fc.id
        );
    END IF;
END $$;

-- =====================================================
-- Step 3: Drop rating_options table if it exists
-- =====================================================
DROP TABLE IF EXISTS rating_options CASCADE;

-- =====================================================
-- Step 4: Add constraint to ensure rating_options is a non-empty array
-- =====================================================
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'chk_form_criteria_rating_options_not_empty'
    ) THEN
        ALTER TABLE form_criteria
        ADD CONSTRAINT chk_form_criteria_rating_options_not_empty
        CHECK (jsonb_array_length(rating_options) > 0);
    END IF;
END $$;

COMMENT ON CONSTRAINT chk_form_criteria_rating_options_not_empty ON form_criteria IS
'Ensures rating_options contains at least one option';
