namespace DoctorAppointmentSystem.DTOs
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
     
        public required string PatientFirstName { get; set; }
        public required string PatientLastName { get; set; }
        public int ProviderId { get; set; }
        public required string ProviderFirstName { get; set; }
        public required string ProviderLastName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public required string Status { get; set; }
        public required string Notes { get; set; }
        public required string Type { get; set; }
    }

  
}
