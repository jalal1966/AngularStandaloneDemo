using AngularStandaloneDemo.Enums;
using DoctorAppointmentSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AngularStandaloneDemo.Models
{
    public class WaitingList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Ensure Identity is set 
        public int Id { get; set; }
        public int PatientId { get; set; }
        public required virtual Patient Patient { get; set; }
        public int ProviderId { get; set; }
        public required virtual User Provider { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public WaitingStatus Status { get; set; }
        public required string Notes { get; set; }
        // Constructor
       
      //  public int AppointmentId { get; set; }
        public virtual required Appointment Appointment { get; set; }
      //  public required ICollection<Appointment> PatientsAppointments { get; set; }
    }
}
