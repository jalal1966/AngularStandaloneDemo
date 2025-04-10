using System;
using System.Collections.Generic;

namespace AngularStandaloneDemo.Models;

public partial class WaitingList
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public int ProviderId { get; set; }

    public DateTime RequestedDate { get; set; }

    public DateTime ExpiryDate { get; set; }

    public int Status { get; set; }

    public string Notes { get; set; } = null!;

    public virtual Appointment IdNavigation { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;

    public virtual User Provider { get; set; } = null!;
}
