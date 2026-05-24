using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class WeeklyConfig
{
    public int WeeklyConfigId { get; set; }

    public int AccountId { get; set; }

    public int Day { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public bool? IsActive { get; set; }

    public virtual Account Account { get; set; } = null!;
}
