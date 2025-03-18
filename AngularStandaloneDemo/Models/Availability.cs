using AngularStandaloneDemo.Enums;

namespace AngularStandaloneDemo.Models
{
    public class Availability
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsRecurring { get; set; }
        public RecurrencePattern RecurrencePattern { get; set; }
    }
}
