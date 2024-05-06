using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaxScheduler.Repository.Migrations
{
    public partial class AddVaccinationCenterIdInCertificateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VaccinationCenterId",
                table: "Certificates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_VaccinationCenterId",
                table: "Certificates",
                column: "VaccinationCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_VaccinationCenters_VaccinationCenterId",
                table: "Certificates",
                column: "VaccinationCenterId",
                principalTable: "VaccinationCenters",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_VaccinationCenters_VaccinationCenterId",
                table: "Certificates");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_VaccinationCenterId",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "VaccinationCenterId",
                table: "Certificates");
        }
    }
}
