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

  
   
    public virtual PatientDetails PatientDetails { get; set; }
}
