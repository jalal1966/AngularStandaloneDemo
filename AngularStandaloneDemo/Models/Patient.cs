using AngularStandaloneDemo.Enums;
using DoctorAppointmentSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularStandaloneDemo.Models
{
    public class Patient
    {
        public Patient()
        {
            Appointments = new List<Appointment>();
            MedicalRecords = new HashSet<MedicalRecord>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensure Identity is set
        public int Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int GenderID { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactNumber { get; set; }
        public string? InsuranceProvider { get; set; }
        public string? InsuranceNumber { get; set; }
        public string? Address { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int? NursID { get; set; }
        public string? NursName { get; set; }
        public string? PatientDoctorName { get; set; }
        public int? PatientDoctorID { get; set; }
        public DateTime? LastVisitDate { get; set; }

        // Navigation properties
        public virtual Gender Gender { get; set; }
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
        public virtual ICollection<Allergy> Allergies { get; set; }
        public virtual ICollection<Medication> Medications { get; set; }
        public virtual ICollection<Visit> Visits { get; set; }
        public virtual ICollection<LabResult> LabResults { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<Immunization> Immunizations { get; set; }
        public virtual ICollection<PatientTask> Tasks { get; set; } = new List<PatientTask>();

        public void UpdateLastVisit(DateTime visitDate)
        {
            LastVisitDate = visitDate;
        }

    }
}
