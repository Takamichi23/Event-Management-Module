namespace appointment_service.DTOs;

public class ProfileCreateDto
{
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
    public string Sex { get; set; } = null!;
    public string? BirthDate { get; set; }
    public string? ContactNumber { get; set; }
    public string? Address { get; set; }
    public string? Street { get; set; }
    public string? Municipality { get; set; }
    public string? Province { get; set; }
    public int ChurchId { get; set; }
    public bool? IsMember { get; set; }
    public string ProfileStatus { get; set; } = "Active";
}

public class ProfileReadDto
{
    public int ProfileId { get; set; }
    public string FirstName { get; set; } = "";
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = "";
    public string Sex { get; set; } = "";
    public string? ContactNumber { get; set; }
    public string? Address { get; set; }
    public bool? IsMember { get; set; }
    public string? BirthDate { get; set; }
    public string ProfileStatus { get; set; } = "";
}
