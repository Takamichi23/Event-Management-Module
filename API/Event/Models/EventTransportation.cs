using System;
using System.Collections.Generic;

namespace event_service.Models;

public partial class EventTransportation
{
    public int TransportationId { get; set; }

    public int EventId { get; set; }

    public string? VehicleType { get; set; }

    public string? Remarks { get; set; }

    public virtual Event Event { get; set; } = null!;
}
