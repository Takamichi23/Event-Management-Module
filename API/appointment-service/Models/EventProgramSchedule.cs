using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class EventProgramSchedule
{
    public int ScheduleId { get; set; }

    public int EventId { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public string? ProgramTitle { get; set; }

    public int DisplayOrder { get; set; }

    public virtual Event Event { get; set; } = null!;
}
