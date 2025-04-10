using System;
using System.Collections.Generic;

namespace AngularStandaloneDemo.Models;

public partial class Immunization
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public string VaccineName { get; set; } = null!;

    public DateTime AdministrationDate { get; set; }

    public string? LotNumber { get; set; }

    public string? AdministeringProvider { get; set; }

    public DateTime NextDoseDate { get; set; }

    public string? Manufacturer { get; set; }

    public virtual Patient Patient { get; set; } = null!;
}
