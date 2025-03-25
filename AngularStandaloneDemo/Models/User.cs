// Models/User.cs
using AngularStandaloneDemo.Enums;
using DoctorAppointmentSystem.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AngularStandaloneDemo.Models
{
    public class User
    {
        public User()
        {
            // Initialize collections to avoid null reference exceptions
            Availabilities = new List<Availability>();
            Appointments = new List<Appointment>();
            AssignedTasks = new List<PatientTask>();  // Initialize collection
            CreatedTasks = new List<PatientTask>();   // Initialize collection
            //Appointments = new List<Appointment>();
            //PatientsAppointments = new List<Appointment>();
            // ProvidersAppointments = new List<Appointment>();
        }
        [Key]
        public int UserID { get; set; } // Explicitly define the primary key
        public required string Username { get; set; }
        [Required]
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public string? Salt { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Specialist { get; set; }
        public required string Address { get; set; }
        public required string TelephoneNo { get; set; }
        public decimal? Salary { get; set; }
        public string? Note { get; set; } // Made nullable to fix CS8618
        public int JobTitleID { get; set; }
        public int GenderID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime LastLoginAt { get; set; }
        public UserRole Role { get; set; }
        // Navigation properties


        // Navigation properties - simplified and non-conflicting
        public ICollection<Availability> Availabilities { get; set; }

        // For users who are providers (doctors/nurses)
        public ICollection<Appointment> Appointments { get; set; }
        public List<PatientTask> AssignedTasks { get; }
        public List<PatientTask> CreatedTasks { get; }
    }

}