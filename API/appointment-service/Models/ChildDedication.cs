using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class ChildDedication
{
    public int ChildDedicationId { get; set; }

    public int RequesterId { get; set; }

    public int AssignedToId { get; set; }

    public int ChildProfileId { get; set; }

    public int? AppointmentId { get; set; }

    public DateTime? ImplementationDate { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    public string? Venue { get; set; }

    public int? CounselId { get; set; }

    public bool? BirthCert { get; set; }

    public string ChildPlaceOfBirth { get; set; } = null!;

    public decimal ChildWeightAtBirth { get; set; }

    public DateTime ParentsMarriageDate { get; set; }

    public string ParentsMarriagePlaceMunicipality { get; set; } = null!;

    public string ParentsMarriagePlaceProvince { get; set; } = null!;

    public string ParentsMarriagePlaceCountry { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual Appointment? Appointment { get; set; }

    public virtual Account AssignedTo { get; set; } = null!;

    public virtual ICollection<ChildDedicationWitness> ChildDedicationWitnesses { get; set; } = new List<ChildDedicationWitness>();

    public virtual Profile ChildProfile { get; set; } = null!;

    public virtual Counsel? Counsel { get; set; }

    public virtual ICollection<ParentProfile> ParentProfiles { get; set; } = new List<ParentProfile>();

    public virtual Account Requester { get; set; } = null!;
}
