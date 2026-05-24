using System;
using System.Collections.Generic;

namespace event_service.Models;

public class BODEvent
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
    public string? OrganizingBody { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string? FundingType { get; set; }
    public decimal ExpectedNumberOfParticipants { get; set; }
    public decimal TicketedPrice { get; set; }
    public decimal AllocatedBudgetPerPerson { get; set; }

    public string? PreparedBy { get; set; }
    public DateTime? DatePrepared { get; set; }
    public string? EndorsedBy { get; set; }
    public string? ApprovedBy { get; set; }
    public string? SignaturePath { get; set; }

    public int? AssignedLeaderId { get; set; }
    public string? AssignedLeaderName { get; set; }

    public int AttendanceCount { get; set; }
    public int WalkInCount { get; set; }
    public int NewMembersCount { get; set; }
    public string? ReportNotes { get; set; }
    public bool IsRescheduled { get; set; }
    public bool IsReportGenerated { get; set; }

    public virtual ICollection<BODProgramSchedule> BODProgramSchedules { get; set; } = new List<BODProgramSchedule>();
    public virtual ICollection<BODGuest> BODGuests { get; set; } = new List<BODGuest>();
    public virtual ICollection<BODPersonnel> BODPersonnel { get; set; } = new List<BODPersonnel>();
    public virtual ICollection<BODEquipment> BODEquipment { get; set; } = new List<BODEquipment>();
    public virtual ICollection<BODTransportation> BODTransportation { get; set; } = new List<BODTransportation>();
    public virtual ICollection<BODExpense> BODExpenses { get; set; } = new List<BODExpense>();
}

public class BODProgramSchedule
{
    public int Id { get; set; }
    public int BODEventId { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? ProgramTitle { get; set; }
    public int DisplayOrder { get; set; }
}

public class BODGuest
{
    public int Id { get; set; }
    public int BODEventId { get; set; }
    public string? GuestType { get; set; }
    public string? FullName { get; set; }
    public string? ContactNumber { get; set; }
}

public class BODPersonnel
{
    public int Id { get; set; }
    public int BODEventId { get; set; }
    public string? RoleName { get; set; }
    public string? FullName { get; set; }
    public string? ContactNumber { get; set; }
}

public class BODEquipment
{
    public int Id { get; set; }
    public int BODEventId { get; set; }
    public string? EquipmentName { get; set; }
    public string? Remarks { get; set; }
}

public class BODTransportation
{
    public int Id { get; set; }
    public int BODEventId { get; set; }
    public string? VehicleType { get; set; }
    public string? Remarks { get; set; }
}

public class BODExpense
{
    public int Id { get; set; }
    public int BODEventId { get; set; }
    public string? ItemName { get; set; }
    public decimal EstimatedCost { get; set; }
}
