using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaxScheduler.Repository.Migrations
{
    public partial class RemoveVaccinationCenterIdFromPatientsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_VaccinationCenters_VaccinationCenterId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_RegisteredPatients_VaccinationCenters_VaccinationCenterId",
                table: "RegisteredPatients");

            migrationBuilder.DropIndex(
                name: "IX_RegisteredPatients_VaccinationCenterId",
                table: "RegisteredPatients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_VaccinationCenterId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "VaccinationCenterId",
                table: "RegisteredPatients");

            migrationBuilder.DropColumn(
                name: "VaccinationCenterId",
                table: "Patients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VaccinationCenterId",
                table: "RegisteredPatients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VaccinationCenterId",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredPatients_VaccinationCenterId",
                table: "RegisteredPatients",
                column: "VaccinationCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_VaccinationCenterId",
                table: "Patients",
                column: "VaccinationCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_VaccinationCenters_VaccinationCenterId",
                table: "Patients",
                column: "VaccinationCenterId",
                principalTable: "VaccinationCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegisteredPatients_VaccinationCenters_VaccinationCenterId",
                table: "RegisteredPatients",
                column: "VaccinationCenterId",
                principalTable: "VaccinationCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
