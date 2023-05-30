using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmoloyeeTask.Data.Migrations.Migrations
{
    public partial class test3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaborCosts_Assignments_IssueId",
                table: "LaborCosts");

            migrationBuilder.DropForeignKey(
                name: "FK_LaborCosts_Employees_EmployeeId",
                table: "LaborCosts");

            migrationBuilder.AlterColumn<int>(
                name: "IssueId",
                table: "LaborCosts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "LaborCosts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LaborCosts_Assignments_IssueId",
                table: "LaborCosts",
                column: "IssueId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LaborCosts_Employees_EmployeeId",
                table: "LaborCosts",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaborCosts_Assignments_IssueId",
                table: "LaborCosts");

            migrationBuilder.DropForeignKey(
                name: "FK_LaborCosts_Employees_EmployeeId",
                table: "LaborCosts");

            migrationBuilder.AlterColumn<int>(
                name: "IssueId",
                table: "LaborCosts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "LaborCosts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_LaborCosts_Assignments_IssueId",
                table: "LaborCosts",
                column: "IssueId",
                principalTable: "Assignments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LaborCosts_Employees_EmployeeId",
                table: "LaborCosts",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
