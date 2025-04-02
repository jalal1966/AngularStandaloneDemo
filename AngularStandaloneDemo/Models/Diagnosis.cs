namespace AngularStandaloneDemo.Models
{
    // Diagnosis Model
    public class Diagnosis
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        public string? DiagnosisCode { get; set; } // ICD code
        public string? Description { get; set; }
        public DateTime DiagnosisDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation property
        public virtual required Visit Visit { get; set; }
    }
}
