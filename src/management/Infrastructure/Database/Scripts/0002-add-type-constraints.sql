-- =====================================================
-- Migration: 0002 - Add Type Constraints
-- Description: Enforces type-specific business rules via CHECK constraints
-- Author: CascVel Team
-- Date: 2025-10-11
-- Compatible with: PostgreSQL 13+
-- Notes: Idempotent - safe to run multiple times
-- =====================================================

-- =====================================================
-- Constraint: Weighted groups must have weight
-- =====================================================
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'chk_form_groups_weight_by_type'
    ) THEN
        ALTER TABLE form_groups
        ADD CONSTRAINT chk_form_groups_weight_by_type
        CHECK (
            weight_basis_points IS NULL
            OR weight_basis_points BETWEEN 0 AND 10000
        );
    END IF;
END $$;

COMMENT ON CONSTRAINT chk_form_groups_weight_by_type ON form_groups IS
'Ensures group weight is either null or between 0 and 10000';

-- =====================================================
-- Constraint: Weighted criteria must have weight
-- =====================================================
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'chk_form_criteria_weight_by_type'
    ) THEN
        ALTER TABLE form_criteria
        ADD CONSTRAINT chk_form_criteria_weight_by_type
        CHECK (
            weight_basis_points IS NULL
            OR weight_basis_points BETWEEN 0 AND 10000
        );
    END IF;
END $$;

COMMENT ON CONSTRAINT chk_form_criteria_weight_by_type ON form_criteria IS
'Ensures criterion weight is either null or between 0 and 10000';

-- =====================================================
-- Constraint: Valid root group types
-- =====================================================
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'chk_forms_valid_root_type'
    ) THEN
        ALTER TABLE forms
        ADD CONSTRAINT chk_forms_valid_root_type
        CHECK (root_group_type IN ('average', 'weighted'));
    END IF;
END $$;

COMMENT ON CONSTRAINT chk_forms_valid_root_type ON forms IS
'Ensures root_group_type has valid discriminator value';

-- =====================================================
-- Constraint: Non-negative order indexes
-- =====================================================
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'chk_form_groups_order_index'
    ) THEN
        ALTER TABLE form_groups
        ADD CONSTRAINT chk_form_groups_order_index
        CHECK (order_index >= 0);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'chk_form_criteria_order_index'
    ) THEN
        ALTER TABLE form_criteria
        ADD CONSTRAINT chk_form_criteria_order_index
        CHECK (order_index >= 0);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'chk_rating_options_order_index'
    ) THEN
        ALTER TABLE rating_options
        ADD CONSTRAINT chk_rating_options_order_index
        CHECK (order_index >= 0);
    END IF;
END $$;

COMMENT ON CONSTRAINT chk_form_groups_order_index ON form_groups IS
'Ensures order_index is non-negative';

COMMENT ON CONSTRAINT chk_form_criteria_order_index ON form_criteria IS
'Ensures order_index is non-negative';

COMMENT ON CONSTRAINT chk_rating_options_order_index ON rating_options IS
'Ensures order_index is non-negative';
