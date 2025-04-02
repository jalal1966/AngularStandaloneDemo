using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AngularStandaloneDemo.Models
{
    // Immunization Model
    public class Immunization
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        [StringLength(100)]
        public string? VaccineName { get; set; }

        [Required]
        public DateTime AdministrationDate { get; set; }

        [StringLength(50)]
        public string? LotNumber { get; set; }

        [StringLength(100)]
        public string? AdministeringProvider { get; set; }

        // Navigation Property
        [ForeignKey("PatientId")]
        public required Patient Patient { get; set; }
    }
}
