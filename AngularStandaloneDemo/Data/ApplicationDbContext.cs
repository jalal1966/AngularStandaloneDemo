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

        public int PatientId { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            // ✅ Ensure the correct table name
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Patient>().ToTable("Patients");
            // Explicit table naming (optional but recommended)
            modelBuilder.Entity<PatientDetails>().ToTable("PatientDetails");
            //modelBuilder.Entity<PatientTask>().ToTable("PatientTasks");


            modelBuilder.Entity<User>()
                .Property(u => u.Salary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<User>()
               .HasKey(u => u.UserID); // Ensure UserID is properly configured as the primary key

            // Configure relationships and constraints
            // FIXED: Remove the duplicate configuration and keep only the more specific one
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Provider)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Configure Patient-PatientDetail relationship (one-to-one)
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.PatientDetails)
                .WithOne(pd => pd.Patient)
                .HasForeignKey<PatientDetails>(pd => pd.PatientId);

            modelBuilder.Entity<WaitingList>()
                .HasKey(w => w.Id); // Ensure primary key is set           


            modelBuilder.Entity<WaitingList>()
                .HasOne(w => w.IdNavigation)
                .WithMany()
                .HasForeignKey(m => m.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Visit>()
             .HasMany(v=> v.Medications)
             .WithOne()
             .HasForeignKey(m => m.Id);



            base.OnModelCreating(modelBuilder); 
        
        }
        public DbSet<AngularStandaloneDemo.Models.Allergy> Allergy { get; set; } = default!;
        public DbSet<AngularStandaloneDemo.Models.Medication> Medication { get; set; } = default!;
        public DbSet<AngularStandaloneDemo.Dtos.PatientInfoDto> PatientInfoDto { get; set; } = default!;
        public DbSet<AngularStandaloneDemo.Models.LabResult> LabResult { get; set; } = default!;
        public DbSet<AngularStandaloneDemo.Models.Immunization> Immunization { get; set; } = default!;
        public DbSet<AngularStandaloneDemo.Models.Visit> Visit { get; set; } = default!;
        
    };
   
}