using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class Counsel
{
    public int CounselId { get; set; }

    public int RequesterId { get; set; }

    public int AssignedToId { get; set; }

    public int ProfileId { get; set; }

    public int? AppointmentId { get; set; }

    public DateTime? ImplementationDate { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    public string? CounselFor { get; set; }

    public string Status { get; set; } = null!;

    public virtual Appointment? Appointment { get; set; }

    public virtual Account AssignedTo { get; set; } = null!;

    public virtual ChildDedication? ChildDedication { get; set; }

    public virtual Profile Profile { get; set; } = null!;

    public virtual Account Requester { get; set; } = null!;

    public virtual Wedding? Wedding { get; set; }
}
