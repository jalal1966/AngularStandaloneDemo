using System;
using System.Collections.Generic;

namespace AngularStandaloneDemo.Models;

public partial class Patient
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

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

    public virtual List<PatientDetails> PatientDetails { get; set; } = new List<PatientDetails>();
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
   
    public virtual ICollection<WaitingList> WaitingLists { get; set; } = new List<WaitingList>();
}
public class PatientBasicInfoUpdate
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DateOfBirth { get; set; }
    public int GenderID { get; set; }
    public int? NursID { get; set; }
    public string NursName { get; set; }
    public int? PatientDoctorID { get; set; }
    public string PatientDoctorName { get; set; }
}

public class PatientBasicInfo
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int GenderID { get; set; }
    public string GenderName { get; set; }
    public string ContactNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string EmergencyContactName { get; set; }
    public string EmergencyContactNumber { get; set; }
    public string InsuranceProvider { get; set; }
    public string InsuranceNumber { get; set; }
    public int? NursID { get; set; }
    public string NursName { get; set; }
    public string PatientDoctorName { get; set; }
    public int? PatientDoctorID { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime? LastVisitDate { get; set; }
}

public class ContactInfoUpdate
{
    public string ContactNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string EmergencyContactName { get; set; }
    public string EmergencyContactNumber { get; set; }
}

public class InsuranceInfoUpdate
{
    public string InsuranceProvider { get; set; }
    public string InsuranceNumber { get; set; }
}
