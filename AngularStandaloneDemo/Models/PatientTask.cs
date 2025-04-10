using System;
using System.Collections.Generic;

namespace AngularStandaloneDemo.Models;

public partial class PatientTask
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int Priority { get; set; }

    public int Status { get; set; }

    public DateTime DueDate { get; set; }

    public int AssignedToNurseId { get; set; }

    public int CreatedByNurseId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? LastModifiedDate { get; set; }

    public DateTime? CompletedDate { get; set; }

    public bool IsRecurring { get; set; }

    public string? RecurringPattern { get; set; }

    public virtual User AssignedToNurse { get; set; } = null!;

    public virtual User CreatedByNurse { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
