using AngularStandaloneDemo.Enums;

namespace AngularStandaloneDemo.Dtos
{
    public class PatientTaskCreateDto
    {
        public int PatientId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime DueDate { get; set; }
        public int AssignedToNurseId { get; set; }
        public int CreatedByNurseId { get; set; }
        public bool IsRecurring { get; set; }
        public required string RecurringPattern { get; set; }
    }

   
}
