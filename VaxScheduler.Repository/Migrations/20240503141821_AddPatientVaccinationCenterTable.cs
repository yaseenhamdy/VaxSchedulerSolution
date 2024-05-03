using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaxScheduler.Repository.Migrations
{
    public partial class AddPatientVaccinationCenterTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumOfDoses",
                table: "patientVaccines");

            migrationBuilder.AddColumn<int>(
                name: "FirstDose",
                table: "patientVaccines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlagFirstDose",
                table: "patientVaccines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlagSecondDose",
                table: "patientVaccines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondDose",
                table: "patientVaccines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PatientVaccinationCenters",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    VaccinationCenterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientVaccinationCenters", x => new { x.VaccinationCenterId, x.PatientId });
                    table.ForeignKey(
                        name: "FK_PatientVaccinationCenters_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientVaccinationCenters_VaccinationCenters_VaccinationCenterId",
                        column: x => x.VaccinationCenterId,
                        principalTable: "VaccinationCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientVaccinationCenters_PatientId",
                table: "PatientVaccinationCenters",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientVaccinationCenters");

            migrationBuilder.DropColumn(
                name: "FirstDose",
                table: "patientVaccines");

            migrationBuilder.DropColumn(
                name: "FlagFirstDose",
                table: "patientVaccines");

            migrationBuilder.DropColumn(
                name: "FlagSecondDose",
                table: "patientVaccines");

            migrationBuilder.DropColumn(
                name: "SecondDose",
                table: "patientVaccines");

            migrationBuilder.AddColumn<int>(
                name: "NumOfDoses",
                table: "patientVaccines",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
