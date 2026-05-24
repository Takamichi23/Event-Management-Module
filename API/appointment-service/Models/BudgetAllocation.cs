using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class BudgetAllocation
{
    public int AllocationId { get; set; }

    public string Name { get; set; } = null!;

    public string Category { get; set; } = null!;

    public decimal Allocated { get; set; }

    public decimal Spent { get; set; }

    public DateTime CreatedAt { get; set; }
}
