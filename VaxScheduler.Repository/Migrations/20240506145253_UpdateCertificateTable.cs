using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaxScheduler.Repository.Migrations
{
    public partial class UpdateCertificateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_VaccinationCenters_VaccinationCenterId",
                table: "Certificates");

            migrationBuilder.RenameColumn(
                name: "VaccinationCenterId",
                table: "Certificates",
                newName: "VaccineId");

            migrationBuilder.RenameIndex(
                name: "IX_Certificates_VaccinationCenterId",
                table: "Certificates",
                newName: "IX_Certificates_VaccineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Vaccines_VaccineId",
                table: "Certificates",
                column: "VaccineId",
                principalTable: "Vaccines",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Vaccines_VaccineId",
                table: "Certificates");

            migrationBuilder.RenameColumn(
                name: "VaccineId",
                table: "Certificates",
                newName: "VaccinationCenterId");

            migrationBuilder.RenameIndex(
                name: "IX_Certificates_VaccineId",
                table: "Certificates",
                newName: "IX_Certificates_VaccinationCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_VaccinationCenters_VaccinationCenterId",
                table: "Certificates",
                column: "VaccinationCenterId",
                principalTable: "VaccinationCenters",
                principalColumn: "Id");
        }
    }
}
