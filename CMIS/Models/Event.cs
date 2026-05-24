using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CMIS.Models
{
    public class EventModel
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Venue { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
        public TimeOnly StartTime { get; set; } = new TimeOnly(9, 0);
        public TimeOnly EndTime { get; set; } = new TimeOnly(17, 0);
        public string? Duration { get; set; }
        public int ExpectedParticipants { get; set; }
        public string? TargetAudience { get; set; }
        public string? OrganizingMinistry { get; set; }
        public string Status { get; set; } = "Planned Event";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public string? FundingType { get; set; }
        public decimal ExpectedNumberOfParticipants { get; set; }
        public decimal TicketedPrice { get; set; }
        public decimal AllocatedBudgetPerPerson { get; set; }

        // Report Fields
        public int AttendanceCount { get; set; }
        public int WalkInCount { get; set; }
        public int NewMembersCount { get; set; }
        public string? ReportNotes { get; set; }
        public bool IsRescheduled { get; set; }
        public bool IsReportGenerated { get; set; }

        [NotMapped]
        public decimal TotalEstimatedBudget => EventExpenses?.Sum(e => e.EstimatedCost) ?? 0;

        public virtual ICollection<ProgramScheduleItem> EventProgramSchedules { get; set; } = new List<ProgramScheduleItem>();
        public virtual ICollection<GuestItem> EventGuests { get; set; } = new List<GuestItem>();
        public virtual ICollection<PersonnelItem> EventPersonnel { get; set; } = new List<PersonnelItem>();
        public virtual ICollection<EquipmentItem> EventEquipments { get; set; } = new List<EquipmentItem>();
        public virtual ICollection<TransportationItem> EventTransportations { get; set; } = new List<TransportationItem>();
        public virtual ICollection<ExpenseItem> EventExpenses { get; set; } = new List<ExpenseItem>();
    }

    public class ProgramScheduleItem
    {
        [Key]
        public int ScheduleId { get; set; }
        public int EventId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? ProgramTitle { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class GuestItem
    {
        [Key]
        public int GuestId { get; set; }
        public int EventId { get; set; }
        public string? GuestType { get; set; }
        public string? FullName { get; set; }
        public string? ContactNumber { get; set; }
    }

    public class PersonnelItem
    {
        [Key]
        public int PersonnelId { get; set; }
        public int EventId { get; set; }
        public string? RoleName { get; set; }
        public string? FullName { get; set; }
        public string? ContactNumber { get; set; }
    }

    public class EquipmentItem
    {
        [Key]
        public int EquipmentId { get; set; }
        public int EventId { get; set; }
        public string? EquipmentName { get; set; }
        public string? Remarks { get; set; }
    }

    public class TransportationItem
    {
        [Key]
        public int TransportationId { get; set; }
        public int EventId { get; set; }
        public string? VehicleType { get; set; }
        public string? Remarks { get; set; }
    }

    public class ExpenseItem
    {
        [Key]
        public int ExpenseId { get; set; }
        public int EventId { get; set; }
        public string? ItemName { get; set; }
        public decimal EstimatedCost { get; set; }
    }
}
