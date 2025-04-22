using System.Text.Json.Serialization;

namespace AngularStandaloneDemo.Models
{
    public class Pressure

    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int MedicalRecordId { get; set; }
    
        // Blood pressure properties
        public int? SystolicPressure { get; set; }
        public int? DiastolicPressure { get; set; }
        public decimal? BloodPressureRatio { get; set; }
        public bool IsBloodPressureNormal { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
       
    }
}
