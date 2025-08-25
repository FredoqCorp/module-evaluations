using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CascVel.Modules.Evaluations.Management.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // evaluation_forms: index on status (from Lifecycle.Status)
            migrationBuilder.CreateIndex(
                name: "ix_evalform_status",
                table: "evaluation_forms",
                column: "status",
                schema: "evaluations");

            // evaluation_forms: unique index on code (from Meta.Code.Value)
            migrationBuilder.CreateIndex(
                name: "ux_evaluation_forms_code",
                table: "evaluation_forms",
                column: "code",
                schema: "evaluations",
                unique: true);

            // form_groups: index on order_index
            migrationBuilder.CreateIndex(
                name: "ix_form_groups_order",
                table: "form_groups",
                column: "order_index",
                schema: "evaluations");

            // form_criteria: index on order_index
            migrationBuilder.CreateIndex(
                name: "ix_form_criteria_order",
                table: "form_criteria",
                column: "order_index",
                schema: "evaluations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_form_criteria_order",
                table: "form_criteria",
                schema: "evaluations");

            migrationBuilder.DropIndex(
                name: "ix_form_groups_order",
                table: "form_groups",
                schema: "evaluations");

            migrationBuilder.DropIndex(
                name: "ux_evaluation_forms_code",
                table: "evaluation_forms",
                schema: "evaluations");

            migrationBuilder.DropIndex(
                name: "ix_evalform_status",
                table: "evaluation_forms",
                schema: "evaluations");
        }
    }
}
