using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class Church
{
    public int ChurchId { get; set; }

    public int DistrictId { get; set; }

    public string ChurchName { get; set; } = null!;

    public string? Address { get; set; }

    public string? ContactNumber { get; set; }

    public string Status { get; set; } = null!;

    public virtual District District { get; set; } = null!;

    public virtual ICollection<LeadershipAssignment> LeadershipAssignments { get; set; } = new List<LeadershipAssignment>();

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}
