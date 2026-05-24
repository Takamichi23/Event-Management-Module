using System;
using System.Collections.Generic;

namespace CMIS.Models;

public partial class ProfileMinistry
{
    public int ProfileMinistryId { get; set; }

    public int ProfileId { get; set; }

    public int MinistryId { get; set; }

    public DateTime? JoinedAt { get; set; }

    public string Status { get; set; } = null!;

    public virtual Ministry Ministry { get; set; } = null!;

    public virtual Profile Profile { get; set; } = null!;
}
