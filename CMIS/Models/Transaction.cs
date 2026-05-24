using System;

namespace CMIS.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public string TransactionCode { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Type { get; set; } = null!; // "Income", "Expense"

    public int? BudgetAllocationId { get; set; }

    public string? BudgetLabel { get; set; } // e.g. "Budget: Music Ministry"

    public decimal Amount { get; set; }

    public int RecordedById { get; set; }

    public DateTime TransactionDate { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation
    public virtual Profile RecordedBy { get; set; } = null!;

    public virtual BudgetAllocation? Allocation { get; set; }
}
