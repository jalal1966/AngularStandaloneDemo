using System.ComponentModel.DataAnnotations;

namespace AngularStandaloneDemo.Dtos
{
    internal class ImmunizationDto
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
        public DateTime NextDoseDate { get; set; }
        public string? Notes { get; set; }
        public string? Manufacturer { get; set; }
    }
}