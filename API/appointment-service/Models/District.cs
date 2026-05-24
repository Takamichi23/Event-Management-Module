using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class District
{
    public int DistrictId { get; set; }

    public string DistrictName { get; set; } = null!;

    public string DistrictCode { get; set; } = null!;

    public string? Address { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Church> Churches { get; set; } = new List<Church>();

    public virtual ICollection<LeadershipAssignment> LeadershipAssignments { get; set; } = new List<LeadershipAssignment>();
}
