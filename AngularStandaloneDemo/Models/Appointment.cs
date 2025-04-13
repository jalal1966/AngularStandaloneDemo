﻿using AngularStandaloneDemo.Enums;
using System;
using System.Collections.Generic;

namespace AngularStandaloneDemo.Models;

public partial class Appointment
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public int ProviderId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int Status { get; set; }

    //public AppointmentStatus AppointmentStatus { get; set; }

    public int Type { get; set; }
   // public AppointmentTypes AppointmentTypes { get; set; }

    public string Notes { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;

    public virtual User Provider { get; set; } = null!;

    // To Do Check 
    public virtual WaitingList? WaitingList { get; set; }
}
