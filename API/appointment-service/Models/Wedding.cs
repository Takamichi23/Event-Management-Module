using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class Wedding
{
    public int WeddingId { get; set; }

    public int RequesterId { get; set; }

    public int AssignedToId { get; set; }

    public int? AppointmentId { get; set; }

    public DateTime? ImplementationDate { get; set; }

    public string? Venue { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    public int? CounselId { get; set; }

    public bool? MarriageCert { get; set; }

    public bool? RecommendationLetter { get; set; }

    public bool? Affidavit { get; set; }

    public string? MarriageType { get; set; }

    public string Status { get; set; } = null!;

    public virtual Appointment? Appointment { get; set; }

    public virtual Account AssignedTo { get; set; } = null!;

    public virtual Counsel? Counsel { get; set; }

    public virtual Account Requester { get; set; } = null!;

    public virtual ICollection<WeddingProfile> WeddingProfiles { get; set; } = new List<WeddingProfile>();

    public virtual ICollection<WeddingWitness> WeddingWitnesses { get; set; } = new List<WeddingWitness>();
}
