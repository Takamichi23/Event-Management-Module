using appointment_service.Models;

namespace appointment_service.DTOs;


public class AppointmentReadDto
{
    public int AppointmentId { get; set; }
    public ServiceType ServiceType { get; set; }
    public AppointmentStatus Status { get; set; }
    public string Date { get; set; }
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public string RequesterName { get; set; } // We combine First + Last Name here
    public string AssignedToName { get; set; } // We combine First + Last Name here

    public string ChurchName { get; set; } = string.Empty;
    public string ChurchAddress { get; set; } = string.Empty;
}