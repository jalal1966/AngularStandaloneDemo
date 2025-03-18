namespace AngularStandaloneDemo.Dtos
{
    public class AvailabilityCreateDto
    {
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsRecurring { get; set; }
        public required string RecurrencePattern { get; set; }
    }
}
