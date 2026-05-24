namespace appointment_service.DTOs;

public class WeddingCreateDto
{
    public int RequesterId { get; set; }
    public int AssignedToId { get; set; }
    public int? AppointmentId { get; set; }
    public string? ImplementationDate { get; set; }
    public string? Venue { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public int? CounselId { get; set; }
    public bool? MarriageCert { get; set; }
    public bool? RecommendationLetter { get; set; }
    public bool? Affidavit { get; set; }
    public string? MarriageType { get; set; }
    public string Status { get; set; } = null!;
    public List<WeddingProfileDto> WeddingProfiles { get; set; } = new();
    public List<int> WeddingWitnessProfileIds { get; set; } = new();
}

public class WeddingProfileDto
{
    public int ProfileId { get; set; }
    public bool? BirthCert { get; set; }
    public string? Occupation { get; set; }
    public string? Religion { get; set; }
    public bool? Cenomar { get; set; }
}
