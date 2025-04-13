using System;
using System.Collections.Generic;

namespace AngularStandaloneDemo.Models;

public partial class Visit
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public DateTime VisitDate { get; set; } = DateTime.Now;

    public string? ProviderName { get; set; }

    public int? ProviderId { get; set; }

    public int VisitType { get; set; } 
    public string? Reason { get; set; }

    public string? Assessment { get; set; }

    public string? PlanTreatment { get; set; }

    public string? Notes { get; set; }

    public bool FollowUpRequired { get; set; }

    public string? FollowUpInstructions { get; set; }

    public DateTime? FollowUpDate { get; set; }

    public string? FollowUpReason { get; set; }

    public string? FollowUpProviderName { get; set; }

    public int? FollowUpProviderId { get; set; }

    public string? FollowUpType { get; set; }

    public int? MedicalRecordId { get; set; }

    public virtual ICollection<Diagnosis> Diagnosis { get; set; } = new List<Diagnosis>();


    public virtual ICollection<Medication> Medication { get; set; } = new List<Medication>();
}


