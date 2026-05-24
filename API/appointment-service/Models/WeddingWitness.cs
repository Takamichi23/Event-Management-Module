using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class WeddingWitness
{
    public int WeddingWitnessId { get; set; }

    public int WeddingId { get; set; }

    public int ProfileId { get; set; }

    public virtual Profile Profile { get; set; } = null!;

    public virtual Wedding Wedding { get; set; } = null!;
}
