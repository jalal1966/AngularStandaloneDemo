using AngularStandaloneDemo.Models;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore;
using AngularStandaloneDemo.Dtos;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Humanizer;

namespace AngularStandaloneDemo.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; } 
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<WaitingList> WaitingList { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientDetails> PatientDetails { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Allergy> Allergies { get; set; }
        
        public DbSet<Medication> Medications { get; set; }
        public DbSet<LabResult> LabResults { get; set; }
        public DbSet<Immunization> Immunizations { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Diagnosis> Diagnosis { get; set; }
        public DbSet<Pressure> Pressure { get; set; }
       
       
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✅ Configure precision and scale for BloodPressureRatio
            modelBuilder.Entity<Pressure>()
                .Property(p => p.BloodPressureRatio)
                .HasColumnType("decimal(5,2)");  // Adjust precision (5) and scale (2) as needed

            // ✅ Table Naming Conventions
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Patient>().ToTable("Patients");
            modelBuilder.Entity<PatientDetails>().ToTable("PatientDetails");

            // ✅ Configure Salary Precision
            modelBuilder.Entity<User>()
                .Property(u => u.Salary)
                .HasPrecision(18, 2);

            // ✅ Primary Keys
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserID);

            // ✅ Appointment: User (Provider) Relationship
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Provider)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Patient ↔ PatientDetails One-to-One
            /*modelBuilder.Entity<Patient>()
                .HasOne(p => p.PatientDetail)
                .WithOne(pd => pd.Patient)
                .HasForeignKey<PatientDetails>(pd => pd.PatientId);*/

            // ✅ WaitingList Navigation (if using same Id as FK)
            modelBuilder.Entity<WaitingList>()
                .HasOne(w => w.IdNavigation)
                .WithMany()
                .HasForeignKey(w => w.Id)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Visit → Medications (One-to-Many)
            modelBuilder.Entity<Visit>()
                .HasMany(v => v.Medication)
                .WithOne(m => m.Visit)
                .HasForeignKey(m => m.VisitId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Visit → Diagnoses (One-to-Many)
            modelBuilder.Entity<Visit>()
                .HasMany(v => v.Diagnosis)
                .WithOne(d => d.Visit)
                .HasForeignKey(d => d.VisitId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Medication → Diagnosis (Many-to-One)
            modelBuilder.Entity<Medication>()
                .HasOne(m => m.Diagnosis)
                .WithMany()
                .HasForeignKey(m => m.DiagnosisId)
                .OnDelete(DeleteBehavior.Restrict);

            // Fix: MedicalRecord to Patient with NoAction delete behavior
            modelBuilder.Entity<MedicalRecord>()
                .HasOne<Patient>()
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(m => m.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            // Visit to MedicalRecord relationship (if needed)
            modelBuilder.Entity<Visit>()
                .HasOne<MedicalRecord>()
                .WithMany(m => m.Visits)
                .HasForeignKey(v => v.MedicalRecordId) // Adjust property name if different
                .OnDelete(DeleteBehavior.NoAction);

            // LabResult to MedicalRecord relationship
            modelBuilder.Entity<LabResult>()
                .HasOne<MedicalRecord>()
                .WithMany(m => m.LabResults)
                .HasForeignKey(l => l.MedicalRecordId)
                .OnDelete(DeleteBehavior.NoAction);

            // Immunization to MedicalRecord relationship
            modelBuilder.Entity<Immunization>()
                .HasOne<MedicalRecord>()
                .WithMany(m => m.Immunizations)
                .HasForeignKey(i => i.MedicalRecordId)
                .OnDelete(DeleteBehavior.NoAction);

            // Pressure to MedicalRecord relationship (removed duplicate)
            modelBuilder.Entity<Pressure>()
                .HasOne<MedicalRecord>()
                .WithMany(m => m.Pressure)
                .HasForeignKey(p => p.MedicalRecordId)
                .OnDelete(DeleteBehavior.NoAction);

            // Allergy to MedicalRecord relationship
            modelBuilder.Entity<Allergy>()
                .HasOne<MedicalRecord>()
                .WithMany(m => m.Allergies)
                .HasForeignKey(a => a.MedicalRecordId) // Adjust property name if different
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }

        internal async Task<bool> UpdateContactInfoAsync(int id, ContactInfoUpdate contactInfo)
        {
            var patient = await Patients.FindAsync(id);
            if (patient == null)
            {
                return false;
            }

            patient.ContactNumber = contactInfo.ContactNumber;
            patient.Email = contactInfo.Email;
            patient.Address = contactInfo.Address;
            patient.EmergencyContactName = contactInfo.EmergencyContactName;
            patient.EmergencyContactNumber = contactInfo.EmergencyContactNumber;

            await SaveChangesAsync();
            return true;
        }

        internal async Task<bool> UpdateInsuranceInfoAsync(int id, InsuranceInfoUpdate insuranceInfo)
        {
            var patient = await Patients.FindAsync(id);
            if (patient == null)
            {
                return false;
            }

            patient.InsuranceProvider = insuranceInfo.InsuranceProvider;
            patient.InsuranceNumber = insuranceInfo.InsuranceNumber;

            await SaveChangesAsync();
            return true;
        }

        internal async Task<bool> UpdatePatientInfoAsync(int id, PatientBasicInfoUpdate patientInfo)
        {
            var patient = await Patients.FindAsync(id);
            if (patient == null)
            {
                return false;
            }

            patient.FirstName = patientInfo.FirstName;
            patient.LastName = patientInfo.LastName;

            if (DateTime.TryParse(patientInfo.DateOfBirth, out var dob))
            {
                patient.DateOfBirth = dob;
            }

            patient.GenderID = patientInfo.GenderID;
            patient.NursID = patientInfo.NursID;
            patient.NursName = patientInfo.NursName;
            patient.PatientDoctorID = patientInfo.PatientDoctorID;
            patient.PatientDoctorName = patientInfo.PatientDoctorName;

            await SaveChangesAsync();
            return true;
        }
    };
   
}