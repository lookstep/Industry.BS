using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmoloyeeTask.Data.Migrations.Migrations
{
    public partial class RenameTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Projects_ProjectId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_LaborCosts_Assignments_IssueId",
                table: "LaborCosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assignments",
                table: "Assignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AllowedIPs",
                table: "AllowedIPs");

            migrationBuilder.RenameTable(
                name: "Assignments",
                newName: "Issues");

            migrationBuilder.RenameTable(
                name: "AllowedIPs",
                newName: "AllowedDeviceCode");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_ProjectId",
                table: "Issues",
                newName: "IX_Issues_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Issues",
                table: "Issues",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllowedDeviceCode",
                table: "AllowedDeviceCode",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Projects_ProjectId",
                table: "Issues",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LaborCosts_Issues_IssueId",
                table: "LaborCosts",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Projects_ProjectId",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_LaborCosts_Issues_IssueId",
                table: "LaborCosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Issues",
                table: "Issues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AllowedDeviceCode",
                table: "AllowedDeviceCode");

            migrationBuilder.RenameTable(
                name: "Issues",
                newName: "Assignments");

            migrationBuilder.RenameTable(
                name: "AllowedDeviceCode",
                newName: "AllowedIPs");

            migrationBuilder.RenameIndex(
                name: "IX_Issues_ProjectId",
                table: "Assignments",
                newName: "IX_Assignments_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assignments",
                table: "Assignments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AllowedIPs",
                table: "AllowedIPs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Projects_ProjectId",
                table: "Assignments",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LaborCosts_Assignments_IssueId",
                table: "LaborCosts",
                column: "IssueId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
