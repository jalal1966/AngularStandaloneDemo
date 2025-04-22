using AngularStandaloneDemo.Models;

namespace AngularStandaloneDemo.Dtos
{
    public class MedicalRecordDto
    {
        public int Id { get; set; }
        public DateTime RecordDate { get; set; }
        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }
        public string? Medications { get; set; }
        public string? Notes { get; set; }
        public bool IsFollowUpRequired { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public int PatientId { get; set; }
        public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
        public virtual ICollection<LabResult> LabResults { get; set; } = new List<LabResult>();
        public virtual ICollection<Allergy> Allergies { get; set; } = new List<Allergy>();
        public virtual ICollection<Immunization> Immunizations { get; set; } = new List<Immunization>();
        public virtual ICollection<Pressure> Pressure { get; set; } = new List<Pressure>();

    }
}
