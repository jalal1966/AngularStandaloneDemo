namespace AngularStandaloneDemo.Models
{
    public class Medication
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string Name { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PrescribingProvider { get; set; }
        public string Purpose { get; set; }
        public bool IsActive { get; set; }

        // Navigation property
        public virtual Patient Patient { get; set; }
    }
}
