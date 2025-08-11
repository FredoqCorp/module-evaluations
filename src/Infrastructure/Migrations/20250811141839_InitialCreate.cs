using CascVel.Module.Evaluations.Management.Domain.Entities.EvaluationCriteria;
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

            migrationBuilder.CreateTable(
                name: "automatic_parameters",
                schema: "evaluations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    caption = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    condition_type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    service_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_automatic_parameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "evaluation_forms",
                schema: "evaluations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    created_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status_changed_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    status_changed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    modified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    valid_from = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    valid_until = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    calculation_rule = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    form_keywords = table.Column<IReadOnlyList<string>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evaluation_forms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "runs",
                schema: "evaluations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    run_for = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    score_result = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    last_saved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_saved_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    first_saved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    first_saved_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    published_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    published_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    context = table.Column<IReadOnlyDictionary<string, string>>(type: "jsonb", nullable: false),
                    evaluation_form_id = table.Column<long>(type: "bigint", nullable: false),
                    evaluation_form_comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    viewed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    agreement_status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    agreement_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_runs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "criteria",
                schema: "evaluations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    weight = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    order_index = table.Column<int>(type: "integer", nullable: false),
                    evaluation_form_id = table.Column<long>(type: "bigint", nullable: false),
                    criterion_type = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    automatic_parameter_id = table.Column<long>(type: "bigint", nullable: true),
                    options = table.Column<IReadOnlyList<EvaluationOption>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_criteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_criteria_automatic_parameters_automatic_parameter_id",
                        column: x => x.automatic_parameter_id,
                        principalSchema: "evaluations",
                        principalTable: "automatic_parameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_criteria_evaluation_forms_evaluation_form_id",
                        column: x => x.evaluation_form_id,
                        principalSchema: "evaluations",
                        principalTable: "evaluation_forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "run_criterion_results",
                schema: "evaluations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    is_skipped = table.Column<bool>(type: "boolean", nullable: false),
                    automatic_value = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    score = table.Column<int>(type: "integer", nullable: true),
                    criterion_id = table.Column<long>(type: "bigint", nullable: true),
                    run_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_run_criterion_results", x => x.Id);
                    table.ForeignKey(
                        name: "FK_run_criterion_results_runs_run_id",
                        column: x => x.run_id,
                        principalSchema: "evaluations",
                        principalTable: "runs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "criterion_children",
                schema: "evaluations",
                columns: table => new
                {
                    parent_id = table.Column<long>(type: "bigint", nullable: false),
                    child_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_criterion_children", x => new { x.parent_id, x.child_id });
                    table.ForeignKey(
                        name: "FK_criterion_children_criteria_child_id",
                        column: x => x.child_id,
                        principalSchema: "evaluations",
                        principalTable: "criteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_criterion_children_criteria_parent_id",
                        column: x => x.parent_id,
                        principalSchema: "evaluations",
                        principalTable: "criteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_automatic_parameters_caption",
                schema: "evaluations",
                table: "automatic_parameters",
                column: "caption");

            migrationBuilder.CreateIndex(
                name: "IX_automatic_parameters_condition_type",
                schema: "evaluations",
                table: "automatic_parameters",
                column: "condition_type");

            migrationBuilder.CreateIndex(
                name: "IX_criteria_automatic_parameter_id",
                schema: "evaluations",
                table: "criteria",
                column: "automatic_parameter_id");

            migrationBuilder.CreateIndex(
                name: "IX_criteria_criterion_type",
                schema: "evaluations",
                table: "criteria",
                column: "criterion_type");

            migrationBuilder.CreateIndex(
                name: "IX_criteria_evaluation_form_id",
                schema: "evaluations",
                table: "criteria",
                column: "evaluation_form_id");

            migrationBuilder.CreateIndex(
                name: "IX_criterion_children_child_id",
                schema: "evaluations",
                table: "criterion_children",
                column: "child_id");

            migrationBuilder.CreateIndex(
                name: "IX_evaluation_forms_code",
                schema: "evaluations",
                table: "evaluation_forms",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_evaluation_forms_status",
                schema: "evaluations",
                table: "evaluation_forms",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_run_criterion_results_criterion_id",
                schema: "evaluations",
                table: "run_criterion_results",
                column: "criterion_id");

            migrationBuilder.CreateIndex(
                name: "IX_run_criterion_results_run_id",
                schema: "evaluations",
                table: "run_criterion_results",
                column: "run_id");

            migrationBuilder.CreateIndex(
                name: "IX_runs_created_at",
                schema: "evaluations",
                table: "runs",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_runs_evaluation_form_id",
                schema: "evaluations",
                table: "runs",
                column: "evaluation_form_id");

            migrationBuilder.CreateIndex(
                name: "IX_runs_run_for",
                schema: "evaluations",
                table: "runs",
                column: "run_for");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "criterion_children",
                schema: "evaluations");

            migrationBuilder.DropTable(
                name: "run_criterion_results",
                schema: "evaluations");

            migrationBuilder.DropTable(
                name: "criteria",
                schema: "evaluations");

            migrationBuilder.DropTable(
                name: "runs",
                schema: "evaluations");

            migrationBuilder.DropTable(
                name: "automatic_parameters",
                schema: "evaluations");

            migrationBuilder.DropTable(
                name: "evaluation_forms",
                schema: "evaluations");
        }
    }
}
