namespace AngularStandaloneDemo.Dtos
{
    public class VitalSignsResponseDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public decimal Temperature { get; set; }
        public int BloodPressureSystolic { get; set; }
        public int BloodPressureDiastolic { get; set; }
        public string BloodPressure => $"{BloodPressureSystolic}/{BloodPressureDiastolic}";
        public int HeartRate { get; set; }
        public int RespiratoryRate { get; set; }
        public int OxygenSaturation { get; set; }
        public decimal? Weight { get; set; }
        public string? Remarks { get; set; }
        public DateTime RecordedAt { get; set; }
        public string Status { get; set; } = "Normal";
    }
}
