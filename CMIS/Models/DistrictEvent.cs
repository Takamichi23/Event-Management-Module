using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CMIS.Models
{
    public class DistrictEventModel
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
        public string? OrganizingDistrict { get; set; }
        public string Status { get; set; } = "Planned Event";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public string? FundingType { get; set; }
        public decimal ExpectedNumberOfParticipants { get; set; }
        public decimal TicketedPrice { get; set; }
        public decimal AllocatedBudgetPerPerson { get; set; }

        // Assignment logic
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
        public decimal TotalEstimatedBudget => DistrictExpenses?.Sum(e => e.EstimatedCost) ?? 0;

        public virtual ICollection<DistrictProgramScheduleItem> DistrictProgramSchedules { get; set; } = new List<DistrictProgramScheduleItem>();
        public virtual ICollection<DistrictGuestItem> DistrictGuests { get; set; } = new List<DistrictGuestItem>();
        public virtual ICollection<DistrictPersonnelItem> DistrictPersonnel { get; set; } = new List<DistrictPersonnelItem>();
        public virtual ICollection<DistrictEquipmentItem> DistrictEquipment { get; set; } = new List<DistrictEquipmentItem>();
        public virtual ICollection<DistrictTransportationItem> DistrictTransportation { get; set; } = new List<DistrictTransportationItem>();
        public virtual ICollection<DistrictExpenseItem> DistrictExpenses { get; set; } = new List<DistrictExpenseItem>();
    }

    public class DistrictProgramScheduleItem
    {
        [Key]
        public int Id { get; set; }
        public int DistrictEventId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? ProgramTitle { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class DistrictGuestItem
    {
        [Key]
        public int Id { get; set; }
        public int DistrictEventId { get; set; }
        public string? GuestType { get; set; }
        public string? FullName { get; set; }
        public string? ContactNumber { get; set; }
    }

    public class DistrictPersonnelItem
    {
        [Key]
        public int Id { get; set; }
        public int DistrictEventId { get; set; }
        public string? RoleName { get; set; }
        public string? FullName { get; set; }
        public string? ContactNumber { get; set; }
    }

    public class DistrictEquipmentItem
    {
        [Key]
        public int Id { get; set; }
        public int DistrictEventId { get; set; }
        public string? EquipmentName { get; set; }
        public string? Remarks { get; set; }
    }

    public class DistrictTransportationItem
    {
        [Key]
        public int Id { get; set; }
        public int DistrictEventId { get; set; }
        public string? VehicleType { get; set; }
        public string? Remarks { get; set; }
    }

    public class DistrictExpenseItem
    {
        [Key]
        public int Id { get; set; }
        public int DistrictEventId { get; set; }
        public string? ItemName { get; set; }
        public decimal EstimatedCost { get; set; }
    }
}
