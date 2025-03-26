using AngularStandaloneDemo.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace AngularStandaloneDemo.Models
{
    public class PatientTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensure Identity is set
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }
        // Make PatientDetails nullable or ensure it's properly initialized
        public PatientDetails? PatientDetails { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public TaskPriority Priority { get; set; }

        [Required]
        public Enums.TaskStatus Status { get; set; }


        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public int AssignedToNurseId { get; set; }

        [Required]
        public int CreatedByNurseId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        [Required]
        public bool IsRecurring { get; set; } = false;

        public string? RecurringPattern { get; set; } // E.g., "daily", "every 4 hours"

        // Foreign Keys
        [ForeignKey(nameof(PatientId))]
        [InverseProperty("Tasks")] // Ensure Patient model has `Tasks` collection
        public virtual Patient? Patient { get; set; }

        [ForeignKey("AssignedToNurseId")]
        public virtual User? AssignedNurse { get; set; }

        [ForeignKey("CreatedByNurseId")]
        public virtual User? CreatedByNurse { get; set; }
        public int PatientDetailsId { get; set; }
    }
}


