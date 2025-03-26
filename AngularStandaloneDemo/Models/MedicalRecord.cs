using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AngularStandaloneDemo.Models
{
    public class MedicalRecord
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensure Identity is set
        public int Id { get; set; }
        public DateTime RecordDate { get; set; }
        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }
        public string? Medications { get; set; }
        public string? Notes { get; set; }
        public bool IsFollowUpRequired { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public int PatientId { get; set; } // Foreign key
        public Patient? Patient { get; set; } // Navigation property
        public int UserID { get; set; } // Doctor/provider who created the record
        public User? User { get; set; }
    }
}
