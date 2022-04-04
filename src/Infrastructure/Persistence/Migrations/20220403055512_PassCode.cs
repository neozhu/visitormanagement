using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Migrations
{
    public partial class PassCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CheckOutDate",
                table: "Visitors",
                newName: "CheckoutDate");

            migrationBuilder.RenameColumn(
                name: "CheckInDate",
                table: "Visitors",
                newName: "CheckinDate");

            migrationBuilder.AddColumn<string>(
                name: "PassCode",
                table: "Visitors",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassCode",
                table: "Visitors");

            migrationBuilder.RenameColumn(
                name: "CheckoutDate",
                table: "Visitors",
                newName: "CheckOutDate");

            migrationBuilder.RenameColumn(
                name: "CheckinDate",
                table: "Visitors",
                newName: "CheckInDate");
        }
    }
}
