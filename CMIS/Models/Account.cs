using System;
using System.Collections.Generic;

namespace CMIS.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public int ProfileId { get; set; }

    public int RoleId { get; set; }

    public int? ChurchId { get; set; }

    public int? DistrictId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Church? Church { get; set; }

    public virtual District? District { get; set; }

    public virtual Profile Profile { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
