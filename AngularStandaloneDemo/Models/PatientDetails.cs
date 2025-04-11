using AngularStandaloneDemo.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularStandaloneDemo.Models
{
    public class PatientDetail
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
        public DateTime? LastVisitDate { get; set; }
        public required string ProfileImageUrl { get; set; }
 
        //public virtual required ICollection<PatientTask> Tasks { get; set; }
        // Navigation properties
        public virtual Gender Gender { get; set; }
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
        public virtual ICollection<Allergy> Allergies { get; set; }
        public virtual ICollection<Medication> Medications { get; set; }
        public virtual ICollection<Visit> Visits { get; set; }
        public virtual ICollection<LabResult> LabResults { get; set; }
        public virtual ICollection<Immunization> Immunizations { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public void UpdateLastVisit(DateTime visitDate)
        {
            LastVisitDate = visitDate;
        }

    }
}
