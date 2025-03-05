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
        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Removed the Gender property configuration as User class does not have a Gender property
            modelBuilder.Entity<User>()
            .Property(u => u.Gender)
            .HasConversion<int>(); // Store the enum as an integer
            base.OnModelCreating(modelBuilder);

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
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public Gender Gender { get; set; }
    }
}