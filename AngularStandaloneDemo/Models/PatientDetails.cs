using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularStandaloneDemo.Models
{
    public class PatientDetails
    {
        [Key]
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
        public virtual Patient? Patient { get; set; }
        public virtual required ICollection<PatientTask> Tasks { get; set; }
    }
}
