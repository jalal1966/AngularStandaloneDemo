using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AngularStandaloneDemo.Models;

public partial class Medication
{
    public int Id { get; set; }

    public int VisitId { get; set; }

    public string? Name { get; set; }

    public string? Dosage { get; set; }

    public string? Frequency { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? PrescribingProvider { get; set; }

    public string? Purpose { get; set; }

    public bool IsActive { get; set; }
    // Prescription related properties
    public int? DiagnosisId { get; set; }
    public bool Refillable { get; set; }
    [Range(0, 12)]
    public int RefillCount { get; set; }
    [MaxLength(500)]
    public string? Instructions { get; set; }
    [MaxLength(2000)]
    public string? PrescriptionNotes { get; set; }

    // Navigation properties
    [JsonIgnore]
    public virtual Visit? Visit { get; set; }

    [ForeignKey("DiagnosisId")]
    [JsonIgnore]
    public virtual Diagnosis? Diagnosis { get; set; }

    // Audit fields
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

}
