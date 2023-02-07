using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuccessfulStartup.Data.Migrations.PlanDb
{
    public partial class UserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessPlans",
                table: "BusinessPlans");

            migrationBuilder.RenameTable(
                name: "BusinessPlans",
                newName: "BusinessPlan");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessPlan",
                table: "BusinessPlan",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessPlan",
                table: "BusinessPlan");

            migrationBuilder.RenameTable(
                name: "BusinessPlan",
                newName: "BusinessPlans");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessPlans",
                table: "BusinessPlans",
                column: "Id");
        }
    }
}
