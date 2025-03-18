using AngularStandaloneDemo.Dtos;
using AngularStandaloneDemo.Enums;
using DoctorAppointmentSystem.DTOs;
using System;

namespace AngularStandaloneDemo.Models
{
    public class UserDto
    {
        public int UserID { get; set; } // Primary key
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Specialist { get; set; }
        public required string Address { get; set; }
        public required string TelephoneNo { get; set; }
        public decimal? Salary { get; set; }
        public string? Note { get; set; } // Nullable
        public int JobTitleID { get; set; }
        public int GenderID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime LastLoginAt { get; set; }
        public UserRole Role { get; set; }

        // Navigation properties (optional, depending on your use case)
        public ICollection<AvailabilityDto> Availabilities { get; set; } = new List<AvailabilityDto>();
        public ICollection<AppointmentDto> PatientsAppointments { get; set; } = new List<AppointmentDto>();
        public ICollection<AppointmentDto> ProvidersAppointments { get; set; } = new List<AppointmentDto>();
    }
}
