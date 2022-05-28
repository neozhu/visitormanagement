using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Migrations
{
    public partial class VisitorId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalHistories_Visitors_VisitId",
                table: "ApprovalHistories");

            migrationBuilder.RenameColumn(
                name: "VisitId",
                table: "ApprovalHistories",
                newName: "VisitorId");

            migrationBuilder.RenameIndex(
                name: "IX_ApprovalHistories_VisitId",
                table: "ApprovalHistories",
                newName: "IX_ApprovalHistories_VisitorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalHistories_Visitors_VisitorId",
                table: "ApprovalHistories",
                column: "VisitorId",
                principalTable: "Visitors",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalHistories_Visitors_VisitorId",
                table: "ApprovalHistories");

            migrationBuilder.RenameColumn(
                name: "VisitorId",
                table: "ApprovalHistories",
                newName: "VisitId");

            migrationBuilder.RenameIndex(
                name: "IX_ApprovalHistories_VisitorId",
                table: "ApprovalHistories",
                newName: "IX_ApprovalHistories_VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalHistories_Visitors_VisitId",
                table: "ApprovalHistories",
                column: "VisitId",
                principalTable: "Visitors",
                principalColumn: "Id");
        }
    }
}
