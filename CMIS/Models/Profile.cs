using System;
using System.Collections.Generic;

namespace CMIS.Models;

public partial class Profile
{
    public int ProfileId { get; set; }

    public int ChurchId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public DateTime? BirthDate { get; set; }

    public string? ContactNumber { get; set; }

    public string? Address { get; set; }

    public string ProfileStatus { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Church Church { get; set; } = null!;

    public virtual ICollection<LeadershipAssignment> LeadershipAssignments { get; set; } = new List<LeadershipAssignment>();

    public virtual ICollection<ProfileMinistry> ProfileMinistries { get; set; } = new List<ProfileMinistry>();
}
