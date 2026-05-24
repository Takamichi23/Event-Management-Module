namespace appointment_service.DTOs;

public class WeeklyConfigCreateDto
{
    public int AccountId { get; set; }
    public int Day { get; set; }
    public string StartTime { get; set; } = null!;
    public string EndTime { get; set; } = null!;
    public bool? IsActive { get; set; }
}
