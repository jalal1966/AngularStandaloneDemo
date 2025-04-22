using System;
using System.Collections.Generic;

namespace AngularStandaloneDemo.Models;

public partial class Allergy
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public int MedicalRecordId { get; set; }
    public string? AllergyType { get; set; }

    public string? Name { get; set; }

    public string? Reaction { get; set; }

    public string? Severity { get; set; }

    public DateTime DateIdentified { get; set; }

    // public virtual Patient Patient { get; set; } = null!;
}
