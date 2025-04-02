namespace AngularStandaloneDemo.Models
{
    // Lab Result Model
    public class LabResult
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime TestDate { get; set; }
        public string? TestName { get; set; }
        public string? Result { get; set; }
        public string? ReferenceRange { get; set; }
        public string? OrderingProvider { get; set; }
        public string? Notes { get; set; }

        // Navigation property
        public virtual required Patient Patient { get; set; }
    }
}
