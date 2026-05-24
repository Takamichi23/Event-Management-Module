using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class Funeral
{
    public int FuneralId { get; set; }

    public int RequesterId { get; set; }

    public int AssignedToId { get; set; }

    public int DeceasedProfileId { get; set; }

    public int KinProfileId { get; set; }

    public int? AppointmentId { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    public DateTime? ImplementationDate { get; set; }

    public string? Venue { get; set; }

    public DateTime DateOfDeath { get; set; }

    public string CauseOfDeath { get; set; } = null!;

    public string? PsaDeathCertNo { get; set; }

    public string KinRelationshipToDeceased { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual Appointment? Appointment { get; set; }

    public virtual Account AssignedTo { get; set; } = null!;

    public virtual Profile DeceasedProfile { get; set; } = null!;

    public virtual Profile KinProfile { get; set; } = null!;

    public virtual Account Requester { get; set; } = null!;
}
