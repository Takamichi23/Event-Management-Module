using System;
using System.Collections.Generic;

namespace CMIS.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<LeadershipAssignment> LeadershipAssignments { get; set; } = new List<LeadershipAssignment>();
}
