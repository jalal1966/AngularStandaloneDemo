using AngularStandaloneDemo.Models;
using DoctorAppointmentSystem.Models;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore;
using AngularStandaloneDemo.Dtos;

namespace AngularStandaloneDemo.Data
{
    public class ApplicationDbContext : DbContext
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
        public DbSet<PatientTask> PatientTasks { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Allergy> Allergies { get; set; }
        
        public DbSet<Medication> Medications { get; set; }
        public DbSet<LabResult> LabResults { get; set; }
        public DbSet<Immunization> Immunizations { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Product> Products { get; set; } = null!;
        public int PatientId { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            // ✅ Ensure the correct table name
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Patient>().ToTable("Patients");
            modelBuilder.Entity<PatientDetails>().ToTable("PatientDetails");
            modelBuilder.Entity<PatientTask>().ToTable("PatientTasks");

            modelBuilder.Entity<User>()
               .HasKey(u => u.UserID); // Ensure UserID is properly configured as the primary key

            // Configure relationships and constraints
            // FIXED: Remove the duplicate configuration and keep only the more specific one
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Provider)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

       
            modelBuilder.Entity<WaitingList>()
                .HasKey(w => w.Id); // Ensure primary key is set           


            modelBuilder.Entity<WaitingList>()
                .HasOne(w => w.Appointment)
                .WithMany()
                .HasForeignKey(m => m.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne<Patient>()
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(m => m.Id);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(m => m.UserID);
           
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 4); // Allows up to 4 decimal places

            // Configure relationships
            // Configure relationships


            modelBuilder.Entity<PatientDetails>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.PatientDetails)
                .HasForeignKey(t => t.PatientDetailsId)
             .IsRequired(false); // Make the relationship optional

            modelBuilder.Entity<PatientTask>()
                .HasOne( pt => pt.Patient)  // Correct navigation property
                .WithMany(p => p.Tasks)  // Ensure Patient has Tasks collection
                .HasForeignKey(pt => pt.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PatientTask>()
                .HasOne(pt => pt.AssignedNurse)
                .WithMany(n => n.AssignedTasks)
                .HasForeignKey(pt => pt.AssignedToNurseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PatientTask>()
                .HasOne(pt => pt.CreatedByNurse)
                .WithMany(n => n.CreatedTasks)
                .HasForeignKey(pt => pt.CreatedByNurseId)
                .OnDelete(DeleteBehavior.Restrict);


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