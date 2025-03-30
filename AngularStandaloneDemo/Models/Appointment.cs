using AngularStandaloneDemo.Enums;
using AngularStandaloneDemo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DoctorAppointmentSystem.Models
{
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensure Identity is set
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int ProviderId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public AppointmentStatus Status { get; set; }
        public AppointmentType Type { get; set; }
        public required string Notes { get; set; }

        // Make navigation properties optional by adding '?'
        public virtual required Patient Patient { get; set; }
        public virtual required User Provider { get; set; }    

    }
}