using System;
using System.Collections.Generic;

namespace AngularStandaloneDemo.Models;

public partial class PatientInfo
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public int GenderId { get; set; }

    public string? ContactNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? EmergencyContactName { get; set; }

    public string? EmergencyContactNumber { get; set; }

    public string? InsuranceProvider { get; set; }

    public string? InsuranceNumber { get; set; }

    public int? NursId { get; set; }

    public string? NursName { get; set; }

    public string? PatientDoctorName { get; set; }

    public int? PatientDoctorId { get; set; }

    public DateTime RegistrationDate { get; set; }

    public DateTime? LastVisitDate { get; set; }
}
