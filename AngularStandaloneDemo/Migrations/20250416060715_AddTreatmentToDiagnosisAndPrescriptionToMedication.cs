using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularStandaloneDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddTreatmentToDiagnosisAndPrescriptionToMedication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabResults_MedicalRecords_MedicalRecordId",
                table: "LabResults");

            migrationBuilder.DropForeignKey(
                name: "FK_LabResults_Patients_PatientId",
                table: "LabResults");

            migrationBuilder.DropIndex(
                name: "IX_LabResults_PatientId",
                table: "LabResults");

            migrationBuilder.RenameColumn(
                name: "MedicalRecordId",
                table: "LabResults",
                newName: "medicalRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_LabResults_MedicalRecordId",
                table: "LabResults",
                newName: "IX_LabResults_medicalRecordId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Medications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DiagnosisId",
                table: "Medications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instructions",
                table: "Medications",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrescriptionNotes",
                table: "Medications",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RefillCount",
                table: "Medications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Refillable",
                table: "Medications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Medications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "medicalRecordId",
                table: "LabResults",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Diagnosis",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FollowUpDate",
                table: "Diagnosis",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FollowUpNeeded",
                table: "Diagnosis",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TreatmentNotes",
                table: "Diagnosis",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TreatmentPlan",
                table: "Diagnosis",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Diagnosis",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medications_DiagnosisId",
                table: "Medications",
                column: "DiagnosisId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabResults_MedicalRecords_medicalRecordId",
                table: "LabResults",
                column: "medicalRecordId",
                principalTable: "MedicalRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_Diagnosis_DiagnosisId",
                table: "Medications",
                column: "DiagnosisId",
                principalTable: "Diagnosis",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabResults_MedicalRecords_medicalRecordId",
                table: "LabResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Medications_Diagnosis_DiagnosisId",
                table: "Medications");

            migrationBuilder.DropIndex(
                name: "IX_Medications_DiagnosisId",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "DiagnosisId",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "Instructions",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "PrescriptionNotes",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "RefillCount",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "Refillable",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Diagnosis");

            migrationBuilder.DropColumn(
                name: "FollowUpDate",
                table: "Diagnosis");

            migrationBuilder.DropColumn(
                name: "FollowUpNeeded",
                table: "Diagnosis");

            migrationBuilder.DropColumn(
                name: "TreatmentNotes",
                table: "Diagnosis");

            migrationBuilder.DropColumn(
                name: "TreatmentPlan",
                table: "Diagnosis");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Diagnosis");

            migrationBuilder.RenameColumn(
                name: "medicalRecordId",
                table: "LabResults",
                newName: "MedicalRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_LabResults_medicalRecordId",
                table: "LabResults",
                newName: "IX_LabResults_MedicalRecordId");

            migrationBuilder.AlterColumn<int>(
                name: "MedicalRecordId",
                table: "LabResults",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_LabResults_PatientId",
                table: "LabResults",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_LabResults_MedicalRecords_MedicalRecordId",
                table: "LabResults",
                column: "MedicalRecordId",
                principalTable: "MedicalRecords",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LabResults_Patients_PatientId",
                table: "LabResults",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
