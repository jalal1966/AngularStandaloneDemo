using AngularStandaloneDemo.Enums;
using System.ComponentModel.DataAnnotations;

namespace AngularStandaloneDemo.Dtos
{
    public class PatientTaskDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskPriority Priority { get; set; }
        public Enums.TaskStatus Status { get; set; }
        public DateTime DueDate { get; set; }
        public int AssignedToUserId { get; set; }
        public string? AssignedToUserName { get; set; }
        public int CreatedByUserId { get; set; }
        public string? CreatedByUserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public bool IsRecurring { get; set; }
        public string? RecurringPattern { get; set; }
        public string? PatientName { get; set; }
    }
}

  

