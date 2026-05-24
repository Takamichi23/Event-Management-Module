using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class BudgetProposal
{
    public int ProposalId { get; set; }

    public string ProposalCode { get; set; } = null!;

    public string Purpose { get; set; } = null!;

    public string? Description { get; set; }

    public int MinistryId { get; set; }

    public string Level { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Status { get; set; } = null!;

    public int SubmittedById { get; set; }

    public int? ReviewedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Ministry Ministry { get; set; } = null!;

    public virtual Profile? ReviewedBy { get; set; }

    public virtual Profile SubmittedBy { get; set; } = null!;
}
