using System;

namespace CMIS.Models;

public partial class Fund
{
    public int FundId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Level { get; set; } = "Church";

    public int? ChurchId { get; set; }

    public int? DistrictId { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Church? Church { get; set; }

    public virtual District? District { get; set; }
}
