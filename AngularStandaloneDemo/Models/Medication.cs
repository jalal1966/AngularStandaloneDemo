using System;
using System.Collections.Generic;

namespace AngularStandaloneDemo.Models;

public partial class Medication
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public string? Name { get; set; }

    public string? Dosage { get; set; }

    public string? Frequency { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? PrescribingProvider { get; set; }

    public string? Purpose { get; set; }

    public bool IsActive { get; set; }

    public virtual Patient Patient { get; set; } = null!;
}
