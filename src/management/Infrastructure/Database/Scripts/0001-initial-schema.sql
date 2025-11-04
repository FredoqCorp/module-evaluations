-- =====================================================
-- Migration: 0001 - Initial Schema
-- Description: Creates initial database schema for evaluation forms module
-- Author: CascVel Team
-- Date: 2025-10-11
-- Compatible with: PostgreSQL 13+
-- Notes: Idempotent - safe to run multiple times
-- =====================================================

-- =====================================================
-- Table: forms
-- Description: Aggregate root for evaluation forms
-- =====================================================
CREATE TABLE IF NOT EXISTS forms
(
    id                uuid         NOT NULL PRIMARY KEY,
    name              varchar(255) NOT NULL,
    description       varchar(1000),
    code              varchar(100) NOT NULL,
    tags              jsonb        NOT NULL,
    root_group_type   varchar(20)  NOT NULL,
    created_at        timestamptz  NOT NULL DEFAULT NOW(),
    updated_at        timestamptz  NOT NULL DEFAULT NOW()
);

-- Create unique index only if it doesn't exist
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_indexes
        WHERE indexname = 'idx_forms_code'
    ) THEN
        CREATE UNIQUE INDEX idx_forms_code ON forms (code);
    END IF;
END $$;

-- Create GIN index only if it doesn't exist
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_indexes
        WHERE indexname = 'idx_forms_tags'
    ) THEN
        CREATE INDEX idx_forms_tags ON forms USING GIN (tags);
    END IF;
END $$;

COMMENT ON TABLE forms IS 'Evaluation form templates with metadata and configuration';
COMMENT ON COLUMN forms.root_group_type IS 'Type of root group: average or weighted';
COMMENT ON COLUMN forms.tags IS 'JSONB array of tags for categorization and search';

-- =====================================================
-- Table: form_groups
-- Description: Hierarchical groups of criteria
-- =====================================================
CREATE TABLE IF NOT EXISTS form_groups
(
    id                   uuid         NOT NULL PRIMARY KEY,
    form_id              uuid         NOT NULL,
    parent_id            uuid,
    title                varchar(255) NOT NULL,
    description          varchar(1000),
    weight_basis_points  int,
    order_index          int          NOT NULL,
    created_at           timestamptz  NOT NULL DEFAULT NOW()
);

-- Add foreign key constraints only if they don't exist
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'fk_form_groups_form_id'
    ) THEN
        ALTER TABLE form_groups
        ADD CONSTRAINT fk_form_groups_form_id
        FOREIGN KEY (form_id)
        REFERENCES forms (id)
        ON DELETE CASCADE;
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'fk_form_groups_parent_id'
    ) THEN
        ALTER TABLE form_groups
        ADD CONSTRAINT fk_form_groups_parent_id
        FOREIGN KEY (parent_id)
        REFERENCES form_groups (id)
        ON DELETE CASCADE;
    END IF;
END $$;

-- Create indexes only if they don't exist
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_indexes WHERE indexname = 'idx_form_groups_form_id') THEN
        CREATE INDEX idx_form_groups_form_id ON form_groups (form_id);
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_indexes WHERE indexname = 'idx_form_groups_parent_id') THEN
        CREATE INDEX idx_form_groups_parent_id ON form_groups (parent_id);
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_indexes WHERE indexname = 'idx_form_groups_order') THEN
        CREATE INDEX idx_form_groups_order ON form_groups (form_id, parent_id, order_index);
    END IF;
END $$;

COMMENT ON TABLE form_groups IS 'Hierarchical groups organizing criteria within forms';
COMMENT ON COLUMN form_groups.weight_basis_points IS 'Weight in basis points (0-10000, only for weighted root forms)';
COMMENT ON COLUMN form_groups.order_index IS 'Display order within parent context';

-- =====================================================
-- Table: form_criteria
-- Description: Evaluation criteria
-- =====================================================
CREATE TABLE IF NOT EXISTS form_criteria
(
    id                   uuid          NOT NULL PRIMARY KEY,
    form_id              uuid          NOT NULL,
    group_id             uuid,
    title                varchar(255)  NOT NULL,
    text                 varchar(2000) NOT NULL,
    weight_basis_points  int,
    order_index          int           NOT NULL,
    created_at           timestamptz   NOT NULL DEFAULT NOW()
);

-- Add foreign key constraints only if they don't exist
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'fk_form_criteria_form_id'
    ) THEN
        ALTER TABLE form_criteria
        ADD CONSTRAINT fk_form_criteria_form_id
        FOREIGN KEY (form_id)
        REFERENCES forms (id)
        ON DELETE CASCADE;
    END IF;

    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'fk_form_criteria_group_id'
    ) THEN
        ALTER TABLE form_criteria
        ADD CONSTRAINT fk_form_criteria_group_id
        FOREIGN KEY (group_id)
        REFERENCES form_groups (id)
        ON DELETE CASCADE;
    END IF;
END $$;

-- Create indexes only if they don't exist
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_indexes WHERE indexname = 'idx_form_criteria_form_id') THEN
        CREATE INDEX idx_form_criteria_form_id ON form_criteria (form_id);
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_indexes WHERE indexname = 'idx_form_criteria_group_id') THEN
        CREATE INDEX idx_form_criteria_group_id ON form_criteria (group_id);
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_indexes WHERE indexname = 'idx_form_criteria_order') THEN
        CREATE INDEX idx_form_criteria_order ON form_criteria (form_id, group_id, order_index);
    END IF;
END $$;

COMMENT ON TABLE form_criteria IS 'Individual evaluation criteria with rating options';
COMMENT ON COLUMN form_criteria.weight_basis_points IS 'Weight in basis points (0-10000, only for weighted root forms)';

-- =====================================================
-- Table: rating_options
-- Description: Rating scale options for criteria
-- =====================================================
CREATE TABLE IF NOT EXISTS rating_options
(
    id            uuid           NOT NULL PRIMARY KEY,
    criterion_id  uuid           NOT NULL,
    score         decimal(5, 2)  NOT NULL,
    label         varchar(100)   NOT NULL,
    annotation    varchar(500),
    order_index   int            NOT NULL,
    created_at    timestamptz    NOT NULL DEFAULT NOW()
);

-- Add foreign key constraint only if it doesn't exist
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint
        WHERE conname = 'fk_rating_options_criterion_id'
    ) THEN
        ALTER TABLE rating_options
        ADD CONSTRAINT fk_rating_options_criterion_id
        FOREIGN KEY (criterion_id)
        REFERENCES form_criteria (id)
        ON DELETE CASCADE;
    END IF;
END $$;

-- Create indexes only if they don't exist
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_indexes WHERE indexname = 'idx_rating_options_criterion_id') THEN
        CREATE INDEX idx_rating_options_criterion_id ON rating_options (criterion_id);
    END IF;

    IF NOT EXISTS (SELECT 1 FROM pg_indexes WHERE indexname = 'idx_rating_options_order') THEN
        CREATE INDEX idx_rating_options_order ON rating_options (criterion_id, order_index);
    END IF;
END $$;

COMMENT ON TABLE rating_options IS 'Available rating options for each criterion';
COMMENT ON COLUMN rating_options.score IS 'Numeric score value for this option';
COMMENT ON COLUMN rating_options.label IS 'Display label for the rating option';
COMMENT ON COLUMN rating_options.annotation IS 'Optional description or help text';
