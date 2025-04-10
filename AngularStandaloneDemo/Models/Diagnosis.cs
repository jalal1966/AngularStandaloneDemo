using System;
using System.Collections.Generic;

namespace AngularStandaloneDemo.Models;

public partial class Diagnosis
{
    public int Id { get; set; }

    public int VisitId { get; set; }

    public string? DiagnosisCode { get; set; }

    public string? Description { get; set; }

    public DateTime DiagnosisDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Visit Visit { get; set; } = null!;
}
