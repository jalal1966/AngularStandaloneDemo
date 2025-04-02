using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularStandaloneDemo.Migrations
{
    /// <inheritdoc />
    public partial class IMMaizMigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Immunization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    VaccineName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AdministrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LotNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AdministeringProvider = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Immunization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Immunization_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientInfoDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GenderID = table.Column<int>(type: "int", nullable: false),
                    GenderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsuranceProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsuranceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NursID = table.Column<int>(type: "int", nullable: true),
                    NursName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientDoctorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientDoctorID = table.Column<int>(type: "int", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastVisitDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientInfoDto", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Immunization_PatientId",
                table: "Immunization",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Immunization");

            migrationBuilder.DropTable(
                name: "PatientInfoDto");
        }
    }
}
