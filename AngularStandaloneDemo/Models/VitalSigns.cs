using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularStandaloneDemo.Models
{
   
        /// <summary>
        /// Represents patient vital signs measurements
        /// </summary>
        public class VitalSigns
        {
            [Key]
            public int Id { get; set; }

            [Required]
            public int PatientId { get; set; }

            [Required]
            [Range(30, 45, ErrorMessage = "Temperature must be between 30°C and 45°C")]
            [Column(TypeName = "decimal(4,1)")]
            public decimal Temperature { get; set; }

            [Required]
            [Range(50, 250, ErrorMessage = "Systolic blood pressure must be between 50 and 250 mmHg")]
            public int BloodPressureSystolic { get; set; }

            [Required]
            [Range(30, 150, ErrorMessage = "Diastolic blood pressure must be between 30 and 150 mmHg")]
            public int BloodPressureDiastolic { get; set; }

            [Required]
            [Range(20, 250, ErrorMessage = "Heart rate must be between 20 and 250 bpm")]
            public int HeartRate { get; set; }

            [Required]
            [Range(5, 60, ErrorMessage = "Respiratory rate must be between 5 and 60 breaths/min")]
            public int RespiratoryRate { get; set; }

            [Required]
            [Range(50, 100, ErrorMessage = "Oxygen saturation must be between 50% and 100%")]
            public int OxygenSaturation { get; set; }

            [Range(0.5, 500, ErrorMessage = "Weight must be between 0.5 and 500 kg")]
            [Column(TypeName = "decimal(5,2)")]
            public decimal? Weight { get; set; }

            [MaxLength(500)]
            public string? Remarks { get; set; }

            [Required]
            public DateTime RecordedAt { get; set; } = DateTime.Now;

            // Navigation property
            [ForeignKey("PatientId")]
            public virtual Patient? Patient { get; set; }
        }
    }

