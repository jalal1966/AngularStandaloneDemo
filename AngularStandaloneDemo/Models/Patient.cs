using System.ComponentModel.DataAnnotations;

namespace AngularStandaloneDemo.Models
{
    public class Patient
    {
        public int Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int GenderID { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactNumber { get; set; }
        public string? InsuranceProvider { get; set; }
        public string? InsuranceNumber { get; set; }
        public int UserID { get; internal set; } // Foreign key to User who manages this patient
        public User? User { get; set; } // Navigation property
        public required ICollection<MedicalRecord> MedicalRecords { get; set; }
    }
}
