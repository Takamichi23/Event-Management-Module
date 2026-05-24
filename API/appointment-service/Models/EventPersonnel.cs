using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class EventPersonnel
{
    public int PersonnelId { get; set; }

    public int EventId { get; set; }

    public string? RoleName { get; set; }

    public string? FullName { get; set; }

    public string? ContactNumber { get; set; }

    public virtual Event Event { get; set; } = null!;
}
