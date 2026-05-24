using System;
using System.Collections.Generic;

namespace event_service.Models;

public partial class EventExpense
{
    public int ExpenseId { get; set; }

    public int FinancialId { get; set; }

    public string? ItemName { get; set; }

    public decimal EstimatedCost { get; set; }

    public virtual Event Financial { get; set; } = null!;
}
