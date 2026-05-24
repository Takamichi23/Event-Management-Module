namespace appointment_service.DTOs;

public class FuneralCreateDto
{
    public int RequesterId { get; set; }
    public int AssignedToId { get; set; }
    public int DeceasedProfileId { get; set; }
    public int KinProfileId { get; set; }
    public int? AppointmentId { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public string? ImplementationDate { get; set; }
    public string? Venue { get; set; }
    public string DateOfDeath { get; set; } = null!;
    public string CauseOfDeath { get; set; } = null!;
    public string? PsaDeathCertNo { get; set; }
    public string KinRelationshipToDeceased { get; set; } = null!;
    public string Status { get; set; } = null!;
}
