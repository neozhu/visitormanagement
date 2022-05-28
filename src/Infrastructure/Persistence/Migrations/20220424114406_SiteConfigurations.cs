using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Migrations
{
    public partial class SiteConfigurations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SiteConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    MandatoryHealthQrCode = table.Column<bool>(type: "bit", nullable: false),
                    MandatoryTripCode = table.Column<bool>(type: "bit", nullable: false),
                    MandatoryNucleicAacidTestReport = table.Column<bool>(type: "bit", nullable: false),
                    Parameters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteConfigurations_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SiteConfigurations_SiteId",
                table: "SiteConfigurations",
                column: "SiteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteConfigurations");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "AspNetUsers");
        }
    }
}
