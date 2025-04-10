using System;
using System.Collections.Generic;

namespace AngularStandaloneDemo.Models;

public partial class PatientDetail
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string RoomNumber { get; set; } = null!;

    public string BedNumber { get; set; } = null!;

    public string PrimaryDiagnosis { get; set; } = null!;

    public DateTime AdmissionDate { get; set; }

    public DateTime? LastVisitDate { get; set; }

    public string ProfileImageUrl { get; set; } = null!;

    public int Gender { get; set; }
}
