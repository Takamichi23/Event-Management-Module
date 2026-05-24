namespace appointment_service.DTOs;

public class DayExceptionCreateDto
{
    public int AccountId { get; set; }
    public string Date { get; set; } = null!;
    public string? Title { get; set; }
    public string? Description { get; set; }
}
