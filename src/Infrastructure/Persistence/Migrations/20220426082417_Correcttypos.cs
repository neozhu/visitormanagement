using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Migrations
{
    public partial class Correcttypos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MandatoryNucleicAacidTestReport",
                table: "SiteConfigurations",
                newName: "MandatoryNucleicAcidTestReport");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MandatoryNucleicAcidTestReport",
                table: "SiteConfigurations",
                newName: "MandatoryNucleicAacidTestReport");
        }
    }
}
