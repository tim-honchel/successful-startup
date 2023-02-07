using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuccessfulStartup.Data.Migrations.PlanDb
{
    public partial class FixUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

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
    }
}
