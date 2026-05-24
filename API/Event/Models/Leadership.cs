using System;
using System.Collections.Generic;

namespace event_service.Models;

public class Profile
{
    public int ProfileId { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Sex { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? ContactNumber { get; set; }
    public string? Address { get; set; }
    public string? ProfileStatus { get; set; }
    public int ChurchId { get; set; }

    public virtual ICollection<LeadershipAssignment> LeadershipAssignments { get; set; } = new List<LeadershipAssignment>();
    public virtual Account? Account { get; set; }
}

public class Account
{
    public int AccountId { get; set; }
    public int ProfileId { get; set; }
    public int RoleId { get; set; }
    public int? ChurchId { get; set; }
    public int? DistrictId { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
}

public class Role
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;
    public string? Description { get; set; }
}

public class LeadershipAssignment
{
    public int AssignmentId { get; set; }
    public int ProfileId { get; set; }
    public int RoleId { get; set; }
    public int? MinistryId { get; set; }
    public int? DistrictId { get; set; }
    public int? ChurchId { get; set; }
    public DateTime AssignedDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = "Active";

    public virtual Profile Profile { get; set; } = null!;
    public virtual Role Role { get; set; } = null!;
    public virtual District? District { get; set; }
    public virtual Church? Church { get; set; }
    public virtual Ministry? Ministry { get; set; }
}

public class Church
{
    public int ChurchId { get; set; }
    public string ChurchName { get; set; } = null!;
    public string? Address { get; set; }
    public int DistrictId { get; set; }
    public string Status { get; set; } = "Active";
}

public class District
{
    public int DistrictId { get; set; }
    public string DistrictName { get; set; } = null!;
    public string? DistrictCode { get; set; }
    public string Status { get; set; } = "Active";
}

public class Ministry
{
    public int MinistryId { get; set; }
    public string MinistryName { get; set; } = null!;
    public string? Description { get; set; }
    public string Status { get; set; } = "Active";
}
