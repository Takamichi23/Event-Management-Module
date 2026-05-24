namespace appointment_service.DTOs;

public class CounselCreateDto
{
    public int RequesterId { get; set; }
    public int AssignedToId { get; set; }
    public int ProfileId { get; set; }
    public int? AppointmentId { get; set; }
    public string? ImplementationDate { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public string? CounselFor { get; set; }
    public string Status { get; set; } = null!;
}
