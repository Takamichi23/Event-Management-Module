using System;
using System.Collections.Generic;

namespace appointment_service.DTOs;

public class AvailableDayDto
{
    public string Date { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }

    public int AssignedToId { get; set; }
}