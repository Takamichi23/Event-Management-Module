namespace appointment_service.DTOs;

public class TimeExceptionCreateDto
{
    public int AccountId { get; set; }
    public string Date { get; set; } = null!;
    public string StartTime { get; set; } = null!;
    public string EndTime { get; set; } = null!;
}
