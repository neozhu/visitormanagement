using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Migrations
{
    public partial class changeCheckinPoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Devices_CheckinPointId",
                table: "Devices");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_CheckinPointId",
                table: "Devices",
                column: "CheckinPointId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Devices_CheckinPointId",
                table: "Devices");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_CheckinPointId",
                table: "Devices",
                column: "CheckinPointId",
                unique: true,
                filter: "[CheckinPointId] IS NOT NULL");
        }
    }
}
