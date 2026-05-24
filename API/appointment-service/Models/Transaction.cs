using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public string TransactionCode { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string AccountName { get; set; } = null!;

    public string? BudgetLabel { get; set; }

    public decimal Amount { get; set; }

    public int RecordedById { get; set; }

    public DateTime TransactionDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Profile RecordedBy { get; set; } = null!;
}
