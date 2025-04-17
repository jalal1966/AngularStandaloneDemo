using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularStandaloneDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddBloodPressureFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medications_Diagnosis_DiagnosisId",
                table: "Medications");

            migrationBuilder.AddColumn<decimal>(
                name: "BloodPressureRatio",
                table: "Diagnosis",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiastolicPressure",
                table: "Diagnosis",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBloodPressureNormal",
                table: "Diagnosis",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SystolicPressure",
                table: "Diagnosis",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_Diagnosis_DiagnosisId",
                table: "Medications",
                column: "DiagnosisId",
                principalTable: "Diagnosis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medications_Diagnosis_DiagnosisId",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "BloodPressureRatio",
                table: "Diagnosis");

            migrationBuilder.DropColumn(
                name: "DiastolicPressure",
                table: "Diagnosis");

            migrationBuilder.DropColumn(
                name: "IsBloodPressureNormal",
                table: "Diagnosis");

            migrationBuilder.DropColumn(
                name: "SystolicPressure",
                table: "Diagnosis");

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_Diagnosis_DiagnosisId",
                table: "Medications",
                column: "DiagnosisId",
                principalTable: "Diagnosis",
                principalColumn: "Id");
        }
    }
}
