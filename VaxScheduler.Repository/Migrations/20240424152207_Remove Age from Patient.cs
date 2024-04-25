using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaxScheduler.API.Migrations
{
    public partial class RemoveAgefromPatient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VaccinationCenters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccinationCenters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VaccinationCenters_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vaccines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DurationBetweenDoses = table.Column<int>(type: "int", nullable: false),
                    Precautions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaccines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vaccines_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
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
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patients_VaccinationCenters_VaccinationCenterId",
                        column: x => x.VaccinationCenterId,
                        principalTable: "VaccinationCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vaccineVaccinationCenters",
                columns: table => new
                {
                    VaccinationCenterId = table.Column<int>(type: "int", nullable: false),
                    VaccineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vaccineVaccinationCenters", x => new { x.VaccinationCenterId, x.VaccineId });
                    table.ForeignKey(
                        name: "FK_vaccineVaccinationCenters_VaccinationCenters_VaccinationCenterId",
                        column: x => x.VaccinationCenterId,
                        principalTable: "VaccinationCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_vaccineVaccinationCenters_Vaccines_VaccineId",
                        column: x => x.VaccineId,
                        principalTable: "Vaccines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    VaccinationCenterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificates_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Certificates_VaccinationCenters_VaccinationCenterId",
                        column: x => x.VaccinationCenterId,
                        principalTable: "VaccinationCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "patientVaccines",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    VaccineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patientVaccines", x => new { x.PatientId, x.VaccineId });
                    table.ForeignKey(
                        name: "FK_patientVaccines_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_patientVaccines_Vaccines_VaccineId",
                        column: x => x.VaccineId,
                        principalTable: "Vaccines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_PatientId",
                table: "Certificates",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_VaccinationCenterId",
                table: "Certificates",
                column: "VaccinationCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_AdminId",
                table: "Patients",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_VaccinationCenterId",
                table: "Patients",
                column: "VaccinationCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_patientVaccines_VaccineId",
                table: "patientVaccines",
                column: "VaccineId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccinationCenters_AdminId",
                table: "VaccinationCenters",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Vaccines_AdminId",
                table: "Vaccines",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_vaccineVaccinationCenters_VaccineId",
                table: "vaccineVaccinationCenters",
                column: "VaccineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "patientVaccines");

            migrationBuilder.DropTable(
                name: "vaccineVaccinationCenters");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Vaccines");

            migrationBuilder.DropTable(
                name: "VaccinationCenters");

            migrationBuilder.DropTable(
                name: "Admins");
        }
    }
}
