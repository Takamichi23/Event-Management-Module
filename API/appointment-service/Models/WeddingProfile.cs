using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class WeddingProfile
{
    public int WeddingProfileId { get; set; }

    public int WeddingId { get; set; }

    public int ProfileId { get; set; }

    public bool? BirthCert { get; set; }

    public string? Occupation { get; set; }

    public string? Religion { get; set; }

    public bool? Cenomar { get; set; }

    public virtual Profile Profile { get; set; } = null!;

    public virtual Wedding Wedding { get; set; } = null!;
}
