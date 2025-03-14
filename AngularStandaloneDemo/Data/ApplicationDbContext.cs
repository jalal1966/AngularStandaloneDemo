using AngularStandaloneDemo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;


namespace AngularStandaloneDemo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }


        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            // Configure Patient and MedicalRecord relationships
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserID);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(m => m.PatientId);

            modelBuilder.Entity<MedicalRecord>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId);


            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 4); // Allows up to 4 decimal places

            // Seed data with static CreatedAt values
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Product 1", Description = "Description of Product 1", Price = 19.99m, CreatedAt = new DateTime(2024, 1, 1, 12, 0, 0) },
                new Product { Id = 2, Name = "Product 2", Description = "Description of Product 2", Price = 29.99m, CreatedAt = new DateTime(2024, 1, 2, 12, 0, 0) },
                new Product { Id = 3, Name = "Product 3", Description = "Description of Product 3", Price = 39.99m, CreatedAt = new DateTime(2024, 1, 3, 12, 0, 0) }
            );
        }
    };
   
}