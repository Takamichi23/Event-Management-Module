using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class Event
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Venue { get; set; }

    public DateTime Date { get; set; }

    public string? Category { get; set; }

    public string? ImageUrl { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public string? Duration { get; set; }

    public int ExpectedParticipants { get; set; }

    public string? TargetAudience { get; set; }

    public string? OrganizingMinistry { get; set; }

    public string Status { get; set; } = null!;

    public string? FundingType { get; set; }

    public decimal ExpectedNumberOfParticipants { get; set; }

    public decimal TicketedPrice { get; set; }

    public decimal AllocatedBudgetPerPerson { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<EventEquipment> EventEquipments { get; set; } = new List<EventEquipment>();

    public virtual ICollection<EventExpense> EventExpenses { get; set; } = new List<EventExpense>();

    public virtual ICollection<EventGuest> EventGuests { get; set; } = new List<EventGuest>();

    public virtual ICollection<EventPersonnel> EventPersonnel { get; set; } = new List<EventPersonnel>();

    public virtual ICollection<EventProgramSchedule> EventProgramSchedules { get; set; } = new List<EventProgramSchedule>();

    public virtual ICollection<EventTransportation> EventTransportations { get; set; } = new List<EventTransportation>();
}
