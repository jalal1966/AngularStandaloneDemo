using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularStandaloneDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientIdToPressure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Pressure",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pressure_PatientId",
                table: "Pressure",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pressure_Patients_PatientId",
                table: "Pressure",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pressure_Patients_PatientId",
                table: "Pressure");

            migrationBuilder.DropIndex(
                name: "IX_Pressure_PatientId",
                table: "Pressure");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Pressure");
        }
    }
}
