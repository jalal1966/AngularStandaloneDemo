namespace AngularStandaloneDemo.Dtos
{
    public class AvailabilityDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsRecurring { get; set; }
        public required string RecurrencePattern { get; set; }
    }
}
