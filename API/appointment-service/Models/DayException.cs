using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class DayException
{
    public int DayExceptionId { get; set; }

    public int AccountId { get; set; }

    public DateTime Date { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public virtual Account Account { get; set; } = null!;
}
