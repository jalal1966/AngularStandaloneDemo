using AngularStandaloneDemo.Models;
using DoctorAppointmentSystem.Models;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            // ✅ Ensure the correct table name
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Patient>().ToTable("Patients");

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
                .HasOne(m => m.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(m => m.PatientId);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserID);
           
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 4); // Allows up to 4 decimal places

            // Configure relationships
            // Configure relationships
            modelBuilder.Entity<PatientTask>()
                .HasOne(pt => pt.PatientDetails)  // Correct navigation property
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
    };
   
}