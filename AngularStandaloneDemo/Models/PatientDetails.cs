using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularStandaloneDemo.Models
{
    public class PatientDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensure Identity is set
        public int Id { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        
        [Required]
        [StringLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public required string LastName { get; set; }

        [Required]
        [StringLength(10)]
        public required string RoomNumber { get; set; }

        [StringLength(10)]
        public required string BedNumber { get; set; }
        
        [StringLength(200)]
        public required string PrimaryDiagnosis { get; set; }

        [Required]
        public DateTime AdmissionDate { get; set; }

        public required string ProfileImageUrl { get; set; }
 
        public virtual required ICollection<PatientTask> Tasks { get; set; }
    }
}
