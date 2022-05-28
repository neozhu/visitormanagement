using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Migrations
{
    public partial class attachements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Attachments",
                table: "VisitorHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelatedAccountId",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckinDateTime",
                table: "Companions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckoutDateTime",
                table: "Companions",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachments",
                table: "VisitorHistories");

            migrationBuilder.DropColumn(
                name: "RelatedAccountId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CheckinDateTime",
                table: "Companions");

            migrationBuilder.DropColumn(
                name: "CheckoutDateTime",
                table: "Companions");
        }
    }
}
