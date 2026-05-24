using System;
using System.Collections.Generic;

namespace CMIS.Models;

public partial class Ministry
{
    public int MinistryId { get; set; }

    public string MinistryName { get; set; } = null!;

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<LeadershipAssignment> LeadershipAssignments { get; set; } = new List<LeadershipAssignment>();

    public virtual ICollection<ProfileMinistry> ProfileMinistries { get; set; } = new List<ProfileMinistry>();
}
