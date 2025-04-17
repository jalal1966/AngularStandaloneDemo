using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AngularStandaloneDemo.Models;

public partial class Diagnosis
{
    public int Id { get; set; }

    public int VisitId { get; set; }

    public string? DiagnosisCode { get; set; }

    public string? Description { get; set; }

    public DateTime DiagnosisDate { get; set; }

    public bool IsActive { get; set; }


    // Treatment related properties
    [MaxLength(2000)]
    public string? TreatmentPlan { get; set; }
    public bool FollowUpNeeded { get; set; }
    [Column(TypeName = "date")]
    public DateTime? FollowUpDate { get; set; }
    [MaxLength(2000)]
    public string? TreatmentNotes { get; set; }

    // Navigation properties
    [JsonIgnore]
    public virtual Visit? Visit { get; set; }

    // Audit fields
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
