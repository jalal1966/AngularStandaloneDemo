using AngularStandaloneDemo.Enums;
using AngularStandaloneDemo.Models;
using DoctorAppointmentSystem.DTOs;

namespace AngularStandaloneDemo.Dtos
{
    // Extended PatientDto with additional information
    public class PatientDetailsDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }

        public string? RoomNumber { get; set; }
        public string? BedNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int GenderID { get; set; }
        public virtual Gender Gender { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactNumber { get; set; }
        public string? InsuranceProvider { get; set; }
        public string? InsuranceNumber { get; set; }
        public int? NursID { get; set; }
        public string? NursName { get; set; }
        public string? PatientDoctorName { get; set; }
        public int? PatientDoctorID { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? LastVisitDate { get; set; }


        // Lists of related records (initialized to empty lists)
        public List<MedicalRecordDto> MedicalRecords { get; set; } = new List<MedicalRecordDto>();
        public List<WaitingListDto> WaitingLists { get; set; } = new List<WaitingListDto>();
        public List<AllergyDto> Allergies { get; set; } = new List<AllergyDto>();
        public List<VisitSummaryDto> RecentVisits { get; set; } = new List<VisitSummaryDto>();

        // Navigation properties as DTOs
        public List<MedicationDto> Medications { get; set; } = new List<MedicationDto>();
        public List<Visit> Visits { get; set; } = new List<Visit>();
        public List<LabResultDto> LabResults { get; set; } = new List<LabResultDto>();
        public List<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();
        public List<Immunization> Immunizations { get; set; } = new List<Immunization>();

        public void UpdateLastVisit(DateTime visitDate)
        {
            LastVisitDate = visitDate;
        }


    }
}
