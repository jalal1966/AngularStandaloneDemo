using AngularStandaloneDemo.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularStandaloneDemo.Models
{
    [Table("PatientTasks")]
    public class PatientTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public TaskPriority Priority { get; set; }

        [Required]
        public Enums.TaskStatus Status { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime DueDate { get; set; }

        [Required]
        public int AssignedToUserId { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime2")]
        public DateTime? LastModifiedDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? CompletedDate { get; set; }

        [Required]
        public bool IsRecurring { get; set; } = false;

        [MaxLength(100)]
        public string? RecurringPattern { get; set; }

        // Navigation properties
        [ForeignKey(nameof(PatientId))]
        public virtual Patient? Patient { get; set; }

        [ForeignKey(nameof(AssignedToUserId))]
        public virtual User? AssignedToUser { get; set; }

        [ForeignKey(nameof(CreatedByUserId))]
        public virtual User? CreatedByUser { get; set; }
    }

}
