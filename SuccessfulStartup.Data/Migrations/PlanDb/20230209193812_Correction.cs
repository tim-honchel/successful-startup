using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuccessfulStartup.Data.Migrations.PlanDb
{
    public partial class Correction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
    name: "PK_Users",
    table: "Users");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "BusinessPlans",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPlans_AuthorId",
                table: "BusinessPlans",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessPlans_Users_AuthorId",
                table: "BusinessPlans",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Cascade);
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                    name: "FK_BusinessPlans_Users_AuthorId",
                    table: "BusinessPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_BusinessPlans_AuthorId",
                table: "BusinessPlans");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "BusinessPlans",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");
        }
    }
    
}
