using System;
using System.Collections.Generic;

namespace CMIS.Models;

public partial class LeadershipAssignment
{
    public int AssignmentId { get; set; }

    public int ProfileId { get; set; }

    public int RoleId { get; set; }

    public int? MinistryId { get; set; }

    public int? ChurchId { get; set; }

    public int? DistrictId { get; set; }

    public DateTime AssignedDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Church? Church { get; set; }

    public virtual District? District { get; set; }

    public virtual Ministry? Ministry { get; set; }

    public virtual Profile Profile { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
