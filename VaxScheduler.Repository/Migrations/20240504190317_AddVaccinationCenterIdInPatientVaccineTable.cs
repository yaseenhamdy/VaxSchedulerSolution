using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaxScheduler.Repository.Migrations
{
    public partial class AddVaccinationCenterIdInPatientVaccineTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_patientVaccines_Vaccines_VaccineId",
                table: "patientVaccines");

            migrationBuilder.AddColumn<int>(
                name: "VaccinationCenterId",
                table: "patientVaccines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_patientVaccines_VaccinationCenterId",
                table: "patientVaccines",
                column: "VaccinationCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_patientVaccines_VaccinationCenters_VaccinationCenterId",
                table: "patientVaccines",
                column: "VaccinationCenterId",
                principalTable: "VaccinationCenters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_patientVaccines_Vaccines_VaccineId",
                table: "patientVaccines",
                column: "VaccineId",
                principalTable: "Vaccines",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_patientVaccines_VaccinationCenters_VaccinationCenterId",
                table: "patientVaccines");

            migrationBuilder.DropForeignKey(
                name: "FK_patientVaccines_Vaccines_VaccineId",
                table: "patientVaccines");

            migrationBuilder.DropIndex(
                name: "IX_patientVaccines_VaccinationCenterId",
                table: "patientVaccines");

            migrationBuilder.DropColumn(
                name: "VaccinationCenterId",
                table: "patientVaccines");

            migrationBuilder.AddForeignKey(
                name: "FK_patientVaccines_Vaccines_VaccineId",
                table: "patientVaccines",
                column: "VaccineId",
                principalTable: "Vaccines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
