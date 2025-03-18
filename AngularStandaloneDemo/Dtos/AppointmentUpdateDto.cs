namespace AngularStandaloneDemo.Dtos
{
    public class AppointmentUpdateDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public required string Status { get; set; }
        public required string Notes { get; set; }
        public required string Type { get; set; }
    }
}
