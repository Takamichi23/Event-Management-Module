using System;

namespace CMIS.Models;

public partial class BudgetProposal
{
    public int ProposalId { get; set; }

    public string ProposalCode { get; set; } = null!;

    public string Purpose { get; set; } = null!;

    public string? Description { get; set; }

    public int? MinistryId { get; set; }

    public int? ChurchId { get; set; }

    public int? DistrictId { get; set; }

    public string Level { get; set; } = null!; // "Ministry", "Church", "District"

    public decimal Amount { get; set; }

    public string Status { get; set; } = null!; // "Pending", "Approved", "Disapproved", "For Revision"

    public string? RejectionReason { get; set; }

    public int SubmittedById { get; set; }

    public int? ReviewedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual Church? Church { get; set; }

    public virtual District? District { get; set; }

    public virtual Ministry? Ministry { get; set; }

    public virtual Profile SubmittedBy { get; set; } = null!;

    public virtual Profile? ReviewedBy { get; set; }
}
