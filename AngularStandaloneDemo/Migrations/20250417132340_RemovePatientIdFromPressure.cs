using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularStandaloneDemo.Migrations
{
    /// <inheritdoc />
    public partial class RemovePatientIdFromPressure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pressure_Patients_PatientId",
                table: "Pressure");

            migrationBuilder.DropForeignKey(
                name: "FK_Pressure_Visits_VisitId",
                table: "Pressure");

            migrationBuilder.DropIndex(
                name: "IX_Pressure_PatientId",
                table: "Pressure");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Pressure");

            migrationBuilder.AddForeignKey(
                name: "FK_Pressure_Patients_VisitId",
                table: "Pressure",
                column: "VisitId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pressure_Visits_VisitId",
                table: "Pressure",
                column: "VisitId",
                principalTable: "Visits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pressure_Patients_VisitId",
                table: "Pressure");

            migrationBuilder.DropForeignKey(
                name: "FK_Pressure_Visits_VisitId",
                table: "Pressure");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Pressure_Visits_VisitId",
                table: "Pressure",
                column: "VisitId",
                principalTable: "Visits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
