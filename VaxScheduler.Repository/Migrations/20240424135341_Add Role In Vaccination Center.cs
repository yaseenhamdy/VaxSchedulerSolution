using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaxScheduler.Repository.Migrations
{
    public partial class AddRoleInVaccinationCenter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "VaccinationCenters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "VaccinationCenters");
        }
    }
}
