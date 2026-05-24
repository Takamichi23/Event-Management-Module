using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int RequesterId { get; set; }

    public int AssignedToId { get; set; }

    public string ServiceType { get; set; } = null!;

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    public DateTime? Date { get; set; }

    public string Status { get; set; } = null!;

    public virtual Account AssignedTo { get; set; } = null!;

    public virtual Baptism? Baptism { get; set; }

    public virtual ChildDedication? ChildDedication { get; set; }

    public virtual Counsel? Counsel { get; set; }

    public virtual Funeral? Funeral { get; set; }

    public virtual Account Requester { get; set; } = null!;

    public virtual Wedding? Wedding { get; set; }
}
