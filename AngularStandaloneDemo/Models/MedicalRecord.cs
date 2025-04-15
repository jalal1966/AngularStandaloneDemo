using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularStandaloneDemo.Models;

public partial class MedicalRecord
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public DateTime RecordDate { get; set; }

    public string? Notes { get; set; }

    public bool IsFollowUpRequired { get; set; }

    public DateTime? FollowUpDate { get; set; }

    public double? Height { get; set; }

    public double? Weight { get; set; }

    public double? Bmi { get; set; }

    public string? BloodType { get; set; }

    public string? ChronicConditions { get; set; }

    public string? SurgicalHistory { get; set; }

    public string? FamilyMedicalHistory { get; set; }

    public string? SocialHistory { get; set; }

  
    public int UserID { get; set; }


    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
    public virtual ICollection<Allergy> Allergies { get; set; } = new List<Allergy>();
    public virtual ICollection<LabResult> LabResults { get; set; } = new List<LabResult>();
  
    public virtual ICollection<Immunization> Immunizations { get; set; } = new List<Immunization>();

}
