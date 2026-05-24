using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class ParentProfile
{
    public int ParentProfileId { get; set; }

    public int ChildDedicationId { get; set; }

    public int ProfileId { get; set; }

    public string Citizenship { get; set; } = null!;

    public string Religion { get; set; } = null!;

    public int TotalNumOfChildren { get; set; }

    public virtual ChildDedication ChildDedication { get; set; } = null!;

    public virtual Profile Profile { get; set; } = null!;
}
