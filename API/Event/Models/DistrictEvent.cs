using System;
using System.Collections.Generic;

namespace event_service.Models;

public class DistrictEvent
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
    public string? OrganizingDistrict { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string? FundingType { get; set; }
    public decimal ExpectedNumberOfParticipants { get; set; }
    public decimal TicketedPrice { get; set; }
    public decimal AllocatedBudgetPerPerson { get; set; }

    public int? AssignedLeaderId { get; set; }
    public string? AssignedLeaderName { get; set; }

    public int AttendanceCount { get; set; }
    public int WalkInCount { get; set; }
    public int NewMembersCount { get; set; }
    public string? ReportNotes { get; set; }
    public bool IsRescheduled { get; set; }
    public bool IsReportGenerated { get; set; }

    public virtual ICollection<DistrictProgramSchedule> DistrictProgramSchedules { get; set; } = new List<DistrictProgramSchedule>();
    public virtual ICollection<DistrictGuest> DistrictGuests { get; set; } = new List<DistrictGuest>();
    public virtual ICollection<DistrictPersonnel> DistrictPersonnel { get; set; } = new List<DistrictPersonnel>();
    public virtual ICollection<DistrictEquipment> DistrictEquipment { get; set; } = new List<DistrictEquipment>();
    public virtual ICollection<DistrictTransportation> DistrictTransportation { get; set; } = new List<DistrictTransportation>();
    public virtual ICollection<DistrictExpense> DistrictExpenses { get; set; } = new List<DistrictExpense>();
}

public class DistrictProgramSchedule
{
    public int Id { get; set; }
    public int DistrictEventId { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? ProgramTitle { get; set; }
    public int DisplayOrder { get; set; }
}

public class DistrictGuest
{
    public int Id { get; set; }
    public int DistrictEventId { get; set; }
    public string? GuestType { get; set; }
    public string? FullName { get; set; }
    public string? ContactNumber { get; set; }
}

public class DistrictPersonnel
{
    public int Id { get; set; }
    public int DistrictEventId { get; set; }
    public string? RoleName { get; set; }
    public string? FullName { get; set; }
    public string? ContactNumber { get; set; }
}

public class DistrictEquipment
{
    public int Id { get; set; }
    public int DistrictEventId { get; set; }
    public string? EquipmentName { get; set; }
    public string? Remarks { get; set; }
}

public class DistrictTransportation
{
    public int Id { get; set; }
    public int DistrictEventId { get; set; }
    public string? VehicleType { get; set; }
    public string? Remarks { get; set; }
}

public class DistrictExpense
{
    public int Id { get; set; }
    public int DistrictEventId { get; set; }
    public string? ItemName { get; set; }
    public decimal EstimatedCost { get; set; }
}
