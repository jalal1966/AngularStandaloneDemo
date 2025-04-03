using Microsoft.CodeAnalysis;

namespace AngularStandaloneDemo.Models
{
    // Visit Model (for tracking appointments and encounters)
    public class Visit
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime VisitDate { get; set; }
        public string? ProviderName { get; set; }
        public int? ProviderId { get; set; }
        public string? VisitType { get; set; } // Primary Care, Specialist, Emergency, etc.
        public string? Reason { get; set; }
        public string? Assessment { get; set; }
        public string? Plan { get; set; }
        public string? Notes { get; set; }

        // Navigation property
        public virtual Patient? Patient { get; set; }
        public virtual required ICollection<Diagnosis> Diagnoses { get; set; }
    }
}
