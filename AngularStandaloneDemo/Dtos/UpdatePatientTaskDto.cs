using AngularStandaloneDemo.Enums;
using System.ComponentModel.DataAnnotations;

namespace AngularStandaloneDemo.Dtos
{
    // DTO for updating an existing task
    // DTO for updating a patient task
    public class UpdatePatientTaskDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public TaskPriority Priority { get; set; }

        [Required]
        public Enums.TaskStatus Status { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public int AssignedToUserId { get; set; }

        public bool IsRecurring { get; set; } = false;

        public string? RecurringPattern { get; set; }
       
    }
}
