using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CMIS.Models
{
    public class BODEventModel
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
        public string? OrganizingBody { get; set; } = "Board of Directors";
        public string Status { get; set; } = "Planned Event";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public string? FundingType { get; set; }
        public decimal ExpectedNumberOfParticipants { get; set; }
        public decimal TicketedPrice { get; set; }
        public decimal AllocatedBudgetPerPerson { get; set; }

        // Approval and Coordination
        public string? PreparedBy { get; set; }
        public DateTime? DatePrepared { get; set; } = DateTime.Now;
        public string? EndorsedBy { get; set; }
        public string? ApprovedBy { get; set; }
        public string? SignaturePath { get; set; }

        // Assignment logic (for Leadership Council to assign National Leader)
        public int? AssignedLeaderId { get; set; }
        public string? AssignedLeaderName { get; set; }

        // Report Fields
        public int AttendanceCount { get; set; }
        public int WalkInCount { get; set; }
        public int NewMembersCount { get; set; }
        public string? ReportNotes { get; set; }
        public bool IsRescheduled { get; set; }
        public bool IsReportGenerated { get; set; }

        [NotMapped]
        public decimal TotalEstimatedBudget => BODExpenses?.Sum(e => e.EstimatedCost) ?? 0;

        public virtual ICollection<BODProgramScheduleItem> BODProgramSchedules { get; set; } = new List<BODProgramScheduleItem>();
        public virtual ICollection<BODGuestItem> BODGuests { get; set; } = new List<BODGuestItem>();
        public virtual ICollection<BODPersonnelItem> BODPersonnel { get; set; } = new List<BODPersonnelItem>();
        public virtual ICollection<BODEquipmentItem> BODEquipment { get; set; } = new List<BODEquipmentItem>();
        public virtual ICollection<BODTransportationItem> BODTransportation { get; set; } = new List<BODTransportationItem>();
        public virtual ICollection<BODExpenseItem> BODExpenses { get; set; } = new List<BODExpenseItem>();
    }

    public class BODProgramScheduleItem
    {
        [Key]
        public int Id { get; set; }
        public int BODEventId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? ProgramTitle { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class BODGuestItem
    {
        [Key]
        public int Id { get; set; }
        public int BODEventId { get; set; }
        public string? GuestType { get; set; }
        public string? FullName { get; set; }
        public string? ContactNumber { get; set; }
    }

    public class BODPersonnelItem
    {
        [Key]
        public int Id { get; set; }
        public int BODEventId { get; set; }
        public string? RoleName { get; set; }
        public string? FullName { get; set; }
        public string? ContactNumber { get; set; }
    }

    public class BODEquipmentItem
    {
        [Key]
        public int Id { get; set; }
        public int BODEventId { get; set; }
        public string? EquipmentName { get; set; }
        public string? Remarks { get; set; }
    }

    public class BODTransportationItem
    {
        [Key]
        public int Id { get; set; }
        public int BODEventId { get; set; }
        public string? VehicleType { get; set; }
        public string? Remarks { get; set; }
    }

    public class BODExpenseItem
    {
        [Key]
        public int Id { get; set; }
        public int BODEventId { get; set; }
        public string? ItemName { get; set; }
        public decimal EstimatedCost { get; set; }
    }
}
