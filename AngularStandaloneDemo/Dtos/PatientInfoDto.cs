using AngularStandaloneDemo.Enums;

namespace AngularStandaloneDemo.Dtos
{
    public class PatientInfoDto
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
    }

    public class ContactInfoUpdateDto
    {
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactNumber { get; set; }
    }

    public class InsuranceUpdateDto
    {
        public string? InsuranceProvider { get; set; }
        public string? InsuranceNumber { get; set; }
    }
}
