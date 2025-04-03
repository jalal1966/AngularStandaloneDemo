using AngularStandaloneDemo.Enums;

namespace AngularStandaloneDemo.Dtos
{
    // Extended PatientDto with additional information
    public class PatientDetailDto
    {
        public int Id { get; set; }
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

        // Medical record summary
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public double? BMI { get; set; }
        public string? BloodType { get; set; }

        // Lists of related records
        public required List<AllergyDto> Allergies { get; set; }
        public required List<MedicationDto> CurrentMedications { get; set; }
        public required List<VisitSummaryDto> RecentVisits { get; set; }
        public required List<LabResultDto> RecentLabResults { get; set; }
        
    }
}
