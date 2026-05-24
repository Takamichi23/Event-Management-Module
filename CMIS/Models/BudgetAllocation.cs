using System;

namespace CMIS.Models;

public partial class BudgetAllocation
{
    public int AllocationId { get; set; }

    public string Name { get; set; } = null!; // "Youth Ministry", "Local Church Operations", etc.

    public string Category { get; set; } = null!; // "Ministry", "Operations"

    public decimal Allocated { get; set; }

    public decimal Spent { get; set; }

    public int? BudgetId { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation
    public virtual Budget? Budget { get; set; }

    /// <summary>
    /// Remaining balance = Allocated - Spent
    /// </summary>
    public decimal RemainingBalance => Allocated - Spent;

    /// <summary>
    /// Utilization percentage
    /// </summary>
    public double Utilization => Allocated > 0 ? (double)(Spent / Allocated) * 100 : 0;
}
