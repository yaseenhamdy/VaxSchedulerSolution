using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaxScheduler.Repository.Migrations
{
	public partial class MakeVaccinationCenterIdOptinal : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<int>(
		name: "VaccinationCenterId",
		table: "RegisteredPatients",
		nullable: true,
		oldClrType: typeof(int),
		oldType: "int",
		oldNullable: false);

			migrationBuilder.AlterColumn<int>(
				name: "VaccinationCenterId",
				table: "Patients",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "int",
				oldNullable: false);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("UPDATE RegisteredPatients SET VaccinationCenterId = 1 WHERE VaccinationCenterId IS NULL");

			// Revert RegisteredPatients to non-nullable
			migrationBuilder.AlterColumn<int>(
				name: "VaccinationCenterId",
				table: "RegisteredPatients",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "int",
				oldNullable: true);

			// Setting default VaccinationCenterId for Patients
			migrationBuilder.Sql("UPDATE Patients SET VaccinationCenterId = 1 WHERE VaccinationCenterId IS NULL");

			// Revert Patients to non-nullable
			migrationBuilder.AlterColumn<int>(
				name: "VaccinationCenterId",
				table: "Patients",
				nullable: false,
				oldClrType: typeof(int),
				oldType: "int",
				oldNullable: true);
		}
	}
}
