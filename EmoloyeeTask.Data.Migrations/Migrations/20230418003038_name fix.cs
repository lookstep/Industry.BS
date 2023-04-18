using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmoloyeeTask.Data.Migrations.Migrations
{
    public partial class namefix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaborCosts_Assignments_AssignmentId",
                table: "LaborCosts");

            migrationBuilder.RenameColumn(
                name: "AssignmentId",
                table: "LaborCosts",
                newName: "IssueId");

            migrationBuilder.RenameIndex(
                name: "IX_LaborCosts_AssignmentId",
                table: "LaborCosts",
                newName: "IX_LaborCosts_IssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_LaborCosts_Assignments_IssueId",
                table: "LaborCosts",
                column: "IssueId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaborCosts_Assignments_IssueId",
                table: "LaborCosts");

            migrationBuilder.RenameColumn(
                name: "IssueId",
                table: "LaborCosts",
                newName: "AssignmentId");

            migrationBuilder.RenameIndex(
                name: "IX_LaborCosts_IssueId",
                table: "LaborCosts",
                newName: "IX_LaborCosts_AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_LaborCosts_Assignments_AssignmentId",
                table: "LaborCosts",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
