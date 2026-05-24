using System;
using System.Collections.Generic;

namespace event_service.Models;

public partial class EventEquipment
{
    public int EquipmentId { get; set; }

    public int EventId { get; set; }

    public string? EquipmentName { get; set; }

    public string? Remarks { get; set; }

    public virtual Event Event { get; set; } = null!;
}
