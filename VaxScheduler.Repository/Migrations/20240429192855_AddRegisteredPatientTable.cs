using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaxScheduler.Repository.Migrations
{
    public partial class AddRegisteredPatientTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RegisteredPatientId",
                table: "patientVaccines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RegisteredPatients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ssn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminId = table.Column<int>(type: "int", nullable: false),
                    VaccinationCenterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredPatients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegisteredPatients_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RegisteredPatients_VaccinationCenters_VaccinationCenterId",
                        column: x => x.VaccinationCenterId,
                        principalTable: "VaccinationCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_patientVaccines_RegisteredPatientId",
                table: "patientVaccines",
                column: "RegisteredPatientId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredPatients_AdminId",
                table: "RegisteredPatients",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredPatients_VaccinationCenterId",
                table: "RegisteredPatients",
                column: "VaccinationCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_patientVaccines_RegisteredPatients_RegisteredPatientId",
                table: "patientVaccines",
                column: "RegisteredPatientId",
                principalTable: "RegisteredPatients",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_patientVaccines_RegisteredPatients_RegisteredPatientId",
                table: "patientVaccines");

            migrationBuilder.DropTable(
                name: "RegisteredPatients");

            migrationBuilder.DropIndex(
                name: "IX_patientVaccines_RegisteredPatientId",
                table: "patientVaccines");

            migrationBuilder.DropColumn(
                name: "RegisteredPatientId",
                table: "patientVaccines");
        }
    }
}
