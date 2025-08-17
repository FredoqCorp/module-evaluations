using System;
using System.Collections.Generic;
using CascVel.Module.Evaluations.Management.Domain.Entities.Criteria;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms;
using CascVel.Module.Evaluations.Management.Domain.Entities.Forms.Calculation;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CascVel.Module.Evaluations.Management.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "evaluations");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:evaluations.form_calculation_kind", "arithmetic_mean,weighted_mean")
                .Annotation("Npgsql:Enum:evaluations.form_status", "archived,draft,published")
                .Annotation("Npgsql:Enum:evaluations.optimization_goal", "maximize,minimize");

            migrationBuilder.CreateTable(
                name: "criteria",
                schema: "evaluations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_criteria", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "evaluation_forms",
                schema: "evaluations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status = table.Column<FormStatus>(type: "evaluations.form_status", nullable: false),
                    valid_from = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    valid_to = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    state_changed_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    state_changed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    calculation_kind = table.Column<FormCalculationKind>(type: "evaluations.form_calculation_kind", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    tags = table.Column<string[]>(type: "text[]", nullable: false, defaultValueSql: "'{}'::text[]"),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evaluation_forms", x => x.id);
                    table.CheckConstraint("ck_eval_forms_validity_presence", "(valid_from IS NULL AND valid_to IS NULL) OR (valid_from IS NOT NULL)");
                    table.CheckConstraint("ck_eval_forms_validity_range", "valid_to IS NULL OR valid_from <= valid_to");
                    table.CheckConstraint("CK_evalform_statechg_all_or_none", "(state_changed_by IS NULL) = (state_changed_at IS NULL)");
                    table.CheckConstraint("CK_evalform_updated_all_or_none", "(updated_by IS NULL) = (updated_at IS NULL)");
                });

            migrationBuilder.CreateTable(
                name: "form_runs",
                schema: "evaluations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    launched_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    launched_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    first_saved_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    first_saved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_saved_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    last_saved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    published_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    published_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    context = table.Column<IReadOnlyDictionary<string, string>>(type: "jsonb", nullable: false),
                    current_total = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    viewed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    agreement_status = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    decided_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    run_for = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    supervisor_comment = table.Column<string>(type: "text", nullable: true),
                    form_code = table.Column<string>(type: "text", nullable: false),
                    form_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_runs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "criterion_automation",
                schema: "evaluations",
                columns: table => new
                {
                    criterion_id = table.Column<long>(type: "bigint", nullable: false),
                    auto_source_key = table.Column<string>(type: "text", nullable: false),
                    auto_goal = table.Column<OptimizationGoal>(type: "evaluations.optimization_goal", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_criterion_automation", x => x.criterion_id);
                    table.ForeignKey(
                        name: "FK_criterion_automation_criteria_criterion_id",
                        column: x => x.criterion_id,
                        principalSchema: "evaluations",
                        principalTable: "criteria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "criterion_options",
                schema: "evaluations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    score = table.Column<short>(type: "smallint", nullable: false),
                    caption = table.Column<string>(type: "text", nullable: true),
                    annotation = table.Column<string>(type: "text", nullable: true),
                    threshold = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    criterion_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_criterion_options", x => x.id);
                    table.ForeignKey(
                        name: "FK_criterion_options_criteria_criterion_id",
                        column: x => x.criterion_id,
                        principalSchema: "evaluations",
                        principalTable: "criteria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_groups",
                schema: "evaluations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    weight = table.Column<short>(type: "smallint", nullable: true),
                    EvaluationFormId = table.Column<long>(type: "bigint", nullable: true),
                    FormGroupId = table.Column<long>(type: "bigint", nullable: true),
                    form_id = table.Column<long>(type: "bigint", nullable: false),
                    parent_id = table.Column<long>(type: "bigint", nullable: true),
                    order_index = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_groups", x => x.id);
                    table.CheckConstraint("CK_form_group_weight", "weight IS NULL OR weight BETWEEN 0 AND 10000");
                    table.CheckConstraint("ck_form_groups_order_non_negative", "order_index >= 0");
                    table.ForeignKey(
                        name: "FK_form_groups_evaluation_forms_EvaluationFormId",
                        column: x => x.EvaluationFormId,
                        principalSchema: "evaluations",
                        principalTable: "evaluation_forms",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_form_groups_evaluation_forms_form_id",
                        column: x => x.form_id,
                        principalSchema: "evaluations",
                        principalTable: "evaluation_forms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_form_groups_form_groups_FormGroupId",
                        column: x => x.FormGroupId,
                        principalSchema: "evaluations",
                        principalTable: "form_groups",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_form_groups_form_groups_parent_id",
                        column: x => x.parent_id,
                        principalSchema: "evaluations",
                        principalTable: "form_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "run_criteria",
                schema: "evaluations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    criterion_id = table.Column<long>(type: "bigint", nullable: false),
                    skipped = table.Column<bool>(type: "boolean", nullable: false),
                    selected_score = table.Column<int>(type: "integer", nullable: true),
                    comment = table.Column<string>(type: "text", nullable: true),
                    auto_parameter_key = table.Column<string>(type: "text", nullable: true),
                    auto_value = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    run_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_run_criteria", x => x.id);
                    table.ForeignKey(
                        name: "FK_run_criteria_form_runs_run_id",
                        column: x => x.run_id,
                        principalSchema: "evaluations",
                        principalTable: "form_runs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_criteria",
                schema: "evaluations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    criterion_id = table.Column<long>(type: "bigint", nullable: false),
                    weight = table.Column<short>(type: "smallint", nullable: true),
                    EvaluationFormId = table.Column<long>(type: "bigint", nullable: true),
                    form_id = table.Column<long>(type: "bigint", nullable: true),
                    group_id = table.Column<long>(type: "bigint", nullable: true),
                    order_index = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_criteria", x => x.id);
                    table.CheckConstraint("ck_form_criteria_order_non_negative", "order_index >= 0");
                    table.CheckConstraint("ck_form_criteria_weight_percent_range", "weight_percent IS NULL OR (weight_percent >= 0 AND weight_percent <= 100)");
                    table.CheckConstraint("CK_formcrit_weight", "weight IS NULL OR weight BETWEEN 0 AND 10000");
                    table.ForeignKey(
                        name: "FK_form_criteria_criteria_criterion_id",
                        column: x => x.criterion_id,
                        principalSchema: "evaluations",
                        principalTable: "criteria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_form_criteria_evaluation_forms_EvaluationFormId",
                        column: x => x.EvaluationFormId,
                        principalSchema: "evaluations",
                        principalTable: "evaluation_forms",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_form_criteria_evaluation_forms_form_id",
                        column: x => x.form_id,
                        principalSchema: "evaluations",
                        principalTable: "evaluation_forms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_form_criteria_form_groups_group_id",
                        column: x => x.group_id,
                        principalSchema: "evaluations",
                        principalTable: "form_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_criterion_options_fk",
                schema: "evaluations",
                table: "criterion_options",
                column: "criterion_id");

            migrationBuilder.CreateIndex(
                name: "ix_criterion_options_score",
                schema: "evaluations",
                table: "criterion_options",
                column: "score");

            migrationBuilder.CreateIndex(
                name: "IX_form_criteria_EvaluationFormId",
                schema: "evaluations",
                table: "form_criteria",
                column: "EvaluationFormId");

            migrationBuilder.CreateIndex(
                name: "ix_form_criteria_fk_criterion",
                schema: "evaluations",
                table: "form_criteria",
                column: "criterion_id");

            migrationBuilder.CreateIndex(
                name: "IX_form_criteria_form_id",
                schema: "evaluations",
                table: "form_criteria",
                column: "form_id");

            migrationBuilder.CreateIndex(
                name: "IX_form_criteria_group_id",
                schema: "evaluations",
                table: "form_criteria",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_form_groups_EvaluationFormId",
                schema: "evaluations",
                table: "form_groups",
                column: "EvaluationFormId");

            migrationBuilder.CreateIndex(
                name: "ix_form_groups_fk_form",
                schema: "evaluations",
                table: "form_groups",
                column: "form_id");

            migrationBuilder.CreateIndex(
                name: "IX_form_groups_FormGroupId",
                schema: "evaluations",
                table: "form_groups",
                column: "FormGroupId");

            migrationBuilder.CreateIndex(
                name: "ix_form_groups_parent",
                schema: "evaluations",
                table: "form_groups",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_run_criteria_unique",
                schema: "evaluations",
                table: "run_criteria",
                columns: new[] { "run_id", "criterion_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "criterion_automation",
                schema: "evaluations");

            migrationBuilder.DropTable(
                name: "criterion_options",
                schema: "evaluations");

            migrationBuilder.DropTable(
                name: "form_criteria",
                schema: "evaluations");

            migrationBuilder.DropTable(
                name: "run_criteria",
                schema: "evaluations");

            migrationBuilder.DropTable(
                name: "criteria",
                schema: "evaluations");

            migrationBuilder.DropTable(
                name: "form_groups",
                schema: "evaluations");

            migrationBuilder.DropTable(
                name: "form_runs",
                schema: "evaluations");

            migrationBuilder.DropTable(
                name: "evaluation_forms",
                schema: "evaluations");
        }
    }
}
