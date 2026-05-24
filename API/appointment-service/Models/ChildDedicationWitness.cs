using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class ChildDedicationWitness
{
    public int ChildDedicationWitnessId { get; set; }

    public int ChildDedicationId { get; set; }

    public int ProfileId { get; set; }

    public virtual ChildDedication ChildDedication { get; set; } = null!;

    public virtual Profile Profile { get; set; } = null!;
}
