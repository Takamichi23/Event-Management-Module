using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class EventGuest
{
    public int GuestId { get; set; }

    public int EventId { get; set; }

    public string? GuestType { get; set; }

    public string? FullName { get; set; }

    public string? ContactNumber { get; set; }

    public virtual Event Event { get; set; } = null!;
}
