using AngularStandaloneDemo.Models;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore;
using AngularStandaloneDemo.Dtos;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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

        //public int PatientId { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

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
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.PatientDetails)
                .WithOne(pd => pd.Patient)
                .HasForeignKey<PatientDetails>(pd => pd.PatientId);


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

            base.OnModelCreating(modelBuilder); 
        
        }

    };
   
}