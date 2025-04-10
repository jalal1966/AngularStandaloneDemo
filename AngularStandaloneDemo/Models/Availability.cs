using System;
using System.Collections.Generic;

namespace AngularStandaloneDemo.Models;

public partial class Availability
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public bool IsRecurring { get; set; }

    public int RecurrencePattern { get; set; }

    public virtual User User { get; set; } = null!;
}
