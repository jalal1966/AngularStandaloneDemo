using System.ComponentModel.DataAnnotations;

namespace AngularStandaloneDemo.Dtos
{
    // DTO for updating just the status
    public class TaskStatusUpdateDto
    {
        [Required]
        public Enums.TaskStatus Status { get; set; }
    }
}
