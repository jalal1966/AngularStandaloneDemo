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

    public int Gender { get; set; }

    public virtual ICollection<Allergy> Allergies { get; set; } = new List<Allergy>();

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Immunization> Immunizations { get; set; } = new List<Immunization>();

    public virtual ICollection<LabResult> LabResults { get; set; } = new List<LabResult>();

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual ICollection<Medication> Medications { get; set; } = new List<Medication>();

    public virtual ICollection<PatientTask> PatientTasks { get; set; } = new List<PatientTask>();

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();

    public virtual ICollection<WaitingList> WaitingLists { get; set; } = new List<WaitingList>();

    internal void UpdateLastVisit(DateTime visitDate)
    {
        throw new NotImplementedException();
    }
}
