namespace AngularStandaloneDemo.Models
{
    // Allergy Model
    public class Allergy
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string? AllergyType { get; set; } // Medication, Food, Environmental, etc.
        public string? Name { get; set; }
        public string? Reaction { get; set; }
        public string? Severity { get; set; }
        public DateTime DateIdentified { get; set; }

        // Navigation property
        // public virtual required Patient Patient { get; set; }
    }
}
