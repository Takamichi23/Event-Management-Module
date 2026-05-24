using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class TimeException
{
    public int TimeExceptionId { get; set; }

    public int AccountId { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public virtual Account Account { get; set; } = null!;
}
