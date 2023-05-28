using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmoloyeeTask.Data.Migrations.Migrations
{
    public partial class ConfirmationCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmationCode",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmationCode",
                table: "Employees");
        }
    }
}
