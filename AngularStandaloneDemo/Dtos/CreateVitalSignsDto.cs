using System.ComponentModel.DataAnnotations;
namespace AngularStandaloneDemo.Dtos
{
    public class CreateVitalSignsDto
    {
        [Required(ErrorMessage = "Patient ID is required")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Temperature is required")]
        [Range(30, 45, ErrorMessage = "Temperature must be between 30°C and 45°C")]
        public decimal Temperature { get; set; }

        [Required(ErrorMessage = "Blood Pressure Systolic is required")]
        [Range(50, 250, ErrorMessage = "Systolic blood pressure must be between 50 and 250 mmHg")]
        public int BloodPressureSystolic { get; set; }

        [Required(ErrorMessage = "Blood Pressure Diastolic is required")]
        [Range(30, 150, ErrorMessage = "Diastolic blood pressure must be between 30 and 150 mmHg")]
        public int BloodPressureDiastolic { get; set; }

        [Required(ErrorMessage = "Heart Rate is required")]
        [Range(20, 250, ErrorMessage = "Heart rate must be between 20 and 250 bpm")]
        public int HeartRate { get; set; }

        [Required(ErrorMessage = "Respiratory Rate is required")]
        [Range(5, 60, ErrorMessage = "Respiratory rate must be between 5 and 60 breaths/min")]
        public int RespiratoryRate { get; set; }

        [Required(ErrorMessage = "Oxygen Saturation is required")]
        [Range(50, 100, ErrorMessage = "Oxygen saturation must be between 50% and 100%")]
        public int OxygenSaturation { get; set; }

        [Range(0.5, 500, ErrorMessage = "Weight must be between 0.5 and 500 kg")]
        public decimal? Weight { get; set; }

        [MaxLength(500, ErrorMessage = "Remarks cannot exceed 500 characters")]
        public string? Remarks { get; set; }
    }
}
