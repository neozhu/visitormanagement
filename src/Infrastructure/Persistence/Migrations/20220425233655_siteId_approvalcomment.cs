using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Migrations
{
    public partial class siteId_approvalcomment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovalComment",
                table: "Visitors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Sites",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SiteId",
                table: "Employees",
                column: "SiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Sites_SiteId",
                table: "Employees",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Sites_SiteId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_SiteId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ApprovalComment",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Employees");
        }
    }
}
