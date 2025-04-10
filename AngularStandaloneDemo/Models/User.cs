using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularStandaloneDemo.Models;

public partial class User
{
    public int UserID { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Salt { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Specialist { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string TelephoneNo { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Salary { get; set; }

    public string? Note { get; set; }

    public int Role { get; set; }

    public int GenderID { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime LastLoginAt { get; set; }

    public int JobTitleID { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Availability> Availabilities { get; set; } = new List<Availability>();

    public virtual ICollection<MedicalRecord> MedicalRecordUserId1Navigations { get; set; } = new List<MedicalRecord>();

    public virtual ICollection<MedicalRecord> MedicalRecordUsers { get; set; } = new List<MedicalRecord>();

    public virtual ICollection<PatientTask> PatientTaskAssignedToNurses { get; set; } = new List<PatientTask>();

    public virtual ICollection<PatientTask> PatientTaskCreatedByNurses { get; set; } = new List<PatientTask>();

    public virtual ICollection<WaitingList> WaitingLists { get; set; } = new List<WaitingList>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; }
}
