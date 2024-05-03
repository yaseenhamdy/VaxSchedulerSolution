using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaxScheduler.Repository.Migrations
{
    public partial class RemovepatientVaccinesFromRigesterdPatientTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_patientVaccines_RegisteredPatients_RegisteredPatientId",
                table: "patientVaccines");

            migrationBuilder.DropIndex(
                name: "IX_patientVaccines_RegisteredPatientId",
                table: "patientVaccines");

            migrationBuilder.DropColumn(
                name: "RegisteredPatientId",
                table: "patientVaccines");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegisteredPatientId",
                table: "patientVaccines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_patientVaccines_RegisteredPatientId",
                table: "patientVaccines",
                column: "RegisteredPatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_patientVaccines_RegisteredPatients_RegisteredPatientId",
                table: "patientVaccines",
                column: "RegisteredPatientId",
                principalTable: "RegisteredPatients",
                principalColumn: "Id");
        }
    }
}
