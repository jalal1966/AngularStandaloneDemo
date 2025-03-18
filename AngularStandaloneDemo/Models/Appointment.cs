using AngularStandaloneDemo.Enums;
using AngularStandaloneDemo.Models;

namespace DoctorAppointmentSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int ProviderId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public AppointmentStatus Status { get; set; }
        public required string Notes { get; set; }
        public AppointmentType Type { get; set; }

        // Make navigation properties optional by adding '?'
        public virtual required Patient Patient { get; set; }
        public virtual required User Provider { get; set; }

        // public int? WaitingListId { get; set; }
       //  public virtual WaitingList? WaitingList { get; set; }

    }


}