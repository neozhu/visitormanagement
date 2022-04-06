using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Migrations
{
    public partial class SiteId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Visitors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_SiteId",
                table: "Visitors",
                column: "SiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Visitors_Sites_SiteId",
                table: "Visitors",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visitors_Sites_SiteId",
                table: "Visitors");

            migrationBuilder.DropIndex(
                name: "IX_Visitors_SiteId",
                table: "Visitors");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Visitors");
        }
    }
}
