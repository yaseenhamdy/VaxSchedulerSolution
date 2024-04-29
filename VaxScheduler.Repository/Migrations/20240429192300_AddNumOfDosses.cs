using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VaxScheduler.Repository.Migrations
{
    public partial class AddNumOfDosses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumOfDoses",
                table: "patientVaccines",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumOfDoses",
                table: "patientVaccines");
        }
    }
}
