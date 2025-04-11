using AngularStandaloneDemo.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularStandaloneDemo.Models;

public partial class PatientDetails
{
    public int Id { get; set; }

    [ForeignKey("Patient")]
    public int PatientId { get; set; }  // Foreign key

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string RoomNumber { get; set; } = null!;

    public string BedNumber { get; set; } = null!;

    public string PrimaryDiagnosis { get; set; } = null!;

    public DateTime AdmissionDate { get; set; }

    public DateTime? LastVisitDate { get; set; }

    public string ProfileImageUrl { get; set; } = null!;

    public virtual Patient Patient { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual ICollection<WaitingList> WaitingLists { get; set; } = new List<WaitingList>();

    internal void UpdateLastVisit(DateTime visitDate)
    {
        throw new NotImplementedException();
    }

}
