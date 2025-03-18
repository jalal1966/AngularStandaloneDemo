namespace AngularStandaloneDemo.Dtos
{
    public class AppointmentCreateDto
    {
        public int PatientId { get; set; }
        public int ProviderId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public required string Notes { get; set; }
        public required string Type { get; set; }
    }
}
