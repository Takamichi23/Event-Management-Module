using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace event_service.Models;

public partial class CmisContext : DbContext
{
    public CmisContext()
    {
    }

    public CmisContext(DbContextOptions<CmisContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }
    public virtual DbSet<EventEquipment> EventEquipments { get; set; }
    public virtual DbSet<EventExpense> EventExpenses { get; set; }
    public virtual DbSet<EventGuest> EventGuests { get; set; }
    public virtual DbSet<EventPersonnel> EventPersonnel { get; set; }
    public virtual DbSet<EventProgramSchedule> EventProgramSchedules { get; set; }
    public virtual DbSet<EventTransportation> EventTransportations { get; set; }

    public virtual DbSet<DistrictEvent> DistrictEvents { get; set; }
    public virtual DbSet<DistrictProgramSchedule> DistrictProgramSchedules { get; set; }
    public virtual DbSet<DistrictGuest> DistrictGuests { get; set; }
    public virtual DbSet<DistrictPersonnel> DistrictPersonnel { get; set; }
    public virtual DbSet<DistrictEquipment> DistrictEquipment { get; set; }
    public virtual DbSet<DistrictTransportation> DistrictTransportation { get; set; }
    public virtual DbSet<DistrictExpense> DistrictExpenses { get; set; }

    public virtual DbSet<BODEvent> BODEvents { get; set; }
    public virtual DbSet<BODProgramSchedule> BODProgramSchedules { get; set; }
    public virtual DbSet<BODGuest> BODGuests { get; set; }
    public virtual DbSet<BODPersonnel> BODPersonnel { get; set; }
    public virtual DbSet<BODEquipment> BODEquipment { get; set; }
    public virtual DbSet<BODTransportation> BODTransportation { get; set; }
    public virtual DbSet<BODExpense> BODExpenses { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }
    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<LeadershipAssignment> LeadershipAssignments { get; set; }
    public virtual DbSet<Church> Churches { get; set; }
    public virtual DbSet<District> Districts { get; set; }
    public virtual DbSet<Ministry> Ministries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL("YOUR_PASSWORD_HERE");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Ministry Event
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("event");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
            entity.Property(e => e.Venue).HasColumnName("venue");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url").HasColumnType("text");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.ExpectedParticipants).HasColumnName("expected_participants");
            entity.Property(e => e.TargetAudience).HasColumnName("target_audience");
            entity.Property(e => e.OrganizingMinistry).HasColumnName("organizing_ministry");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.FundingType).HasColumnName("funding_type");
            entity.Property(e => e.ExpectedNumberOfParticipants).HasColumnName("expected_number_of_participants").HasColumnType("decimal(15,2)");
            entity.Property(e => e.TicketedPrice).HasColumnName("ticketed_price").HasColumnType("decimal(15,2)");
            entity.Property(e => e.AllocatedBudgetPerPerson).HasColumnName("allocated_budget_per_person").HasColumnType("decimal(15,2)");
            entity.Property(e => e.AttendanceCount).HasColumnName("attendance_count");
            entity.Property(e => e.WalkInCount).HasColumnName("walk_in_count");
            entity.Property(e => e.NewMembersCount).HasColumnName("new_members_count");
            entity.Property(e => e.ReportNotes).HasColumnName("report_notes").HasColumnType("text");
            entity.Property(e => e.IsRescheduled).HasColumnName("is_rescheduled");
            entity.Property(e => e.IsReportGenerated).HasColumnName("is_report_generated");

            entity.HasMany(e => e.EventProgramSchedules).WithOne(p => p.Event).HasForeignKey(p => p.EventId);
            entity.HasMany(e => e.EventGuests).WithOne(g => g.Event).HasForeignKey(g => g.EventId);
            entity.HasMany(e => e.EventPersonnel).WithOne(p => p.Event).HasForeignKey(p => p.EventId);
            entity.HasMany(e => e.EventEquipments).WithOne(e => e.Event).HasForeignKey(e => e.EventId);
            entity.HasMany(e => e.EventTransportations).WithOne(t => t.Event).HasForeignKey(t => t.EventId);
            entity.HasMany(e => e.EventExpenses).WithOne(ex => ex.Financial).HasForeignKey(ex => ex.FinancialId);
        });

        modelBuilder.Entity<EventEquipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentId);
            entity.ToTable("event_equipment");
            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.EquipmentName).HasColumnName("equipment_name");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
        });

        modelBuilder.Entity<EventExpense>(entity =>
        {
            entity.HasKey(e => e.ExpenseId);
            entity.ToTable("event_expense");
            entity.Property(e => e.ExpenseId).HasColumnName("expense_id");
            entity.Property(e => e.FinancialId).HasColumnName("financial_id");
            entity.Property(e => e.ItemName).HasColumnName("item_name");
            entity.Property(e => e.EstimatedCost).HasColumnName("estimated_cost").HasColumnType("decimal(15,2)");
        });

        modelBuilder.Entity<EventGuest>(entity =>
        {
            entity.HasKey(e => e.GuestId);
            entity.ToTable("event_guest");
            entity.Property(e => e.GuestId).HasColumnName("guest_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.GuestType).HasColumnName("guest_type");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.ContactNumber).HasColumnName("contact_number");
        });

        modelBuilder.Entity<EventPersonnel>(entity =>
        {
            entity.HasKey(e => e.PersonnelId);
            entity.ToTable("event_personnel");
            entity.Property(e => e.PersonnelId).HasColumnName("personnel_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.RoleName).HasColumnName("role_name");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.ContactNumber).HasColumnName("contact_number");
        });

        modelBuilder.Entity<EventProgramSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId);
            entity.ToTable("event_program_schedule");
            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.ProgramTitle).HasColumnName("program_title");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
        });

        modelBuilder.Entity<EventTransportation>(entity =>
        {
            entity.HasKey(e => e.TransportationId);
            entity.ToTable("event_transportation");
            entity.Property(e => e.TransportationId).HasColumnName("transportation_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.VehicleType).HasColumnName("vehicle_type");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
        });

        // District Event
        modelBuilder.Entity<DistrictEvent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("district_event");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
            entity.Property(e => e.Venue).HasColumnName("venue");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url").HasColumnType("text");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.ExpectedParticipants).HasColumnName("expected_participants");
            entity.Property(e => e.TargetAudience).HasColumnName("target_audience");
            entity.Property(e => e.OrganizingDistrict).HasColumnName("organizing_district");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.FundingType).HasColumnName("funding_type");
            entity.Property(e => e.ExpectedNumberOfParticipants).HasColumnName("expected_number_of_participants").HasColumnType("decimal(15,2)");
            entity.Property(e => e.TicketedPrice).HasColumnName("ticketed_price").HasColumnType("decimal(15,2)");
            entity.Property(e => e.AllocatedBudgetPerPerson).HasColumnName("allocated_budget_per_person").HasColumnType("decimal(15,2)");
            entity.Property(e => e.AssignedLeaderId).HasColumnName("assigned_leader_id");
            entity.Property(e => e.AssignedLeaderName).HasColumnName("assigned_leader_name");
            entity.Property(e => e.AttendanceCount).HasColumnName("attendance_count");
            entity.Property(e => e.WalkInCount).HasColumnName("walk_in_count");
            entity.Property(e => e.NewMembersCount).HasColumnName("new_members_count");
            entity.Property(e => e.ReportNotes).HasColumnName("report_notes").HasColumnType("text");
            entity.Property(e => e.IsRescheduled).HasColumnName("is_rescheduled");
            entity.Property(e => e.IsReportGenerated).HasColumnName("is_report_generated");

            entity.HasMany(e => e.DistrictProgramSchedules).WithOne().HasForeignKey(p => p.DistrictEventId);
            entity.HasMany(e => e.DistrictGuests).WithOne().HasForeignKey(g => g.DistrictEventId);
            entity.HasMany(e => e.DistrictPersonnel).WithOne().HasForeignKey(p => p.DistrictEventId);
            entity.HasMany(e => e.DistrictEquipment).WithOne().HasForeignKey(e => e.DistrictEventId);
            entity.HasMany(e => e.DistrictTransportation).WithOne().HasForeignKey(t => t.DistrictEventId);
            entity.HasMany(e => e.DistrictExpenses).WithOne().HasForeignKey(ex => ex.DistrictEventId);
        });

        modelBuilder.Entity<DistrictProgramSchedule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("district_event_program_schedule");
            entity.Property(e => e.Id).HasColumnName("schedule_id");
            entity.Property(e => e.DistrictEventId).HasColumnName("district_event_id");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.ProgramTitle).HasColumnName("program_title");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
        });

        modelBuilder.Entity<DistrictGuest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("district_event_guest");
            entity.Property(e => e.Id).HasColumnName("guest_id");
            entity.Property(e => e.DistrictEventId).HasColumnName("district_event_id");
            entity.Property(e => e.GuestType).HasColumnName("guest_type");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.ContactNumber).HasColumnName("contact_number");
        });

        modelBuilder.Entity<DistrictPersonnel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("district_event_personnel");
            entity.Property(e => e.Id).HasColumnName("personnel_id");
            entity.Property(e => e.DistrictEventId).HasColumnName("district_event_id");
            entity.Property(e => e.RoleName).HasColumnName("role_name");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.ContactNumber).HasColumnName("contact_number");
        });

        modelBuilder.Entity<DistrictEquipment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("district_event_equipment");
            entity.Property(e => e.Id).HasColumnName("equipment_id");
            entity.Property(e => e.DistrictEventId).HasColumnName("district_event_id");
            entity.Property(e => e.EquipmentName).HasColumnName("equipment_name");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
        });

        modelBuilder.Entity<DistrictTransportation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("district_event_transportation");
            entity.Property(e => e.Id).HasColumnName("transportation_id");
            entity.Property(e => e.DistrictEventId).HasColumnName("district_event_id");
            entity.Property(e => e.VehicleType).HasColumnName("vehicle_type");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
        });

        modelBuilder.Entity<DistrictExpense>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("district_event_expense");
            entity.Property(e => e.Id).HasColumnName("expense_id");
            entity.Property(e => e.DistrictEventId).HasColumnName("district_event_id");
            entity.Property(e => e.ItemName).HasColumnName("item_name");
            entity.Property(e => e.EstimatedCost).HasColumnName("estimated_cost").HasColumnType("decimal(15,2)");
        });

        // BOD Event
        modelBuilder.Entity<BODEvent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("bod_event");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
            entity.Property(e => e.Venue).HasColumnName("venue");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url").HasColumnType("text");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.ExpectedParticipants).HasColumnName("expected_participants");
            entity.Property(e => e.TargetAudience).HasColumnName("target_audience");
            entity.Property(e => e.OrganizingBody).HasColumnName("organizing_body");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.FundingType).HasColumnName("funding_type");
            entity.Property(e => e.ExpectedNumberOfParticipants).HasColumnName("expected_number_of_participants").HasColumnType("decimal(15,2)");
            entity.Property(e => e.TicketedPrice).HasColumnName("ticketed_price").HasColumnType("decimal(15,2)");
            entity.Property(e => e.AllocatedBudgetPerPerson).HasColumnName("allocated_budget_per_person").HasColumnType("decimal(15,2)");
            entity.Property(e => e.PreparedBy).HasColumnName("prepared_by");
            entity.Property(e => e.DatePrepared).HasColumnName("date_prepared");
            entity.Property(e => e.EndorsedBy).HasColumnName("endorsed_by");
            entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
            entity.Property(e => e.SignaturePath).HasColumnName("signature_path");
            entity.Property(e => e.AssignedLeaderId).HasColumnName("assigned_leader_id");
            entity.Property(e => e.AssignedLeaderName).HasColumnName("assigned_leader_name");
            entity.Property(e => e.AttendanceCount).HasColumnName("attendance_count");
            entity.Property(e => e.WalkInCount).HasColumnName("walk_in_count");
            entity.Property(e => e.NewMembersCount).HasColumnName("new_members_count");
            entity.Property(e => e.ReportNotes).HasColumnName("report_notes").HasColumnType("text");
            entity.Property(e => e.IsRescheduled).HasColumnName("is_rescheduled");
            entity.Property(e => e.IsReportGenerated).HasColumnName("is_report_generated");

            entity.HasMany(e => e.BODProgramSchedules).WithOne().HasForeignKey(p => p.BODEventId);
            entity.HasMany(e => e.BODGuests).WithOne().HasForeignKey(g => g.BODEventId);
            entity.HasMany(e => e.BODPersonnel).WithOne().HasForeignKey(p => p.BODEventId);
            entity.HasMany(e => e.BODEquipment).WithOne().HasForeignKey(e => e.BODEventId);
            entity.HasMany(e => e.BODTransportation).WithOne().HasForeignKey(t => t.BODEventId);
            entity.HasMany(e => e.BODExpenses).WithOne().HasForeignKey(ex => ex.BODEventId);
        });

        modelBuilder.Entity<BODProgramSchedule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("bod_event_program_schedule");
            entity.Property(e => e.Id).HasColumnName("schedule_id");
            entity.Property(e => e.BODEventId).HasColumnName("bod_event_id");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.ProgramTitle).HasColumnName("program_title");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
        });

        modelBuilder.Entity<BODGuest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("bod_event_guest");
            entity.Property(e => e.Id).HasColumnName("guest_id");
            entity.Property(e => e.BODEventId).HasColumnName("bod_event_id");
            entity.Property(e => e.GuestType).HasColumnName("guest_type");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.ContactNumber).HasColumnName("contact_number");
        });

        modelBuilder.Entity<BODPersonnel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("bod_event_personnel");
            entity.Property(e => e.Id).HasColumnName("personnel_id");
            entity.Property(e => e.BODEventId).HasColumnName("bod_event_id");
            entity.Property(e => e.RoleName).HasColumnName("role_name");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.ContactNumber).HasColumnName("contact_number");
        });

        modelBuilder.Entity<BODEquipment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("bod_event_equipment");
            entity.Property(e => e.Id).HasColumnName("equipment_id");
            entity.Property(e => e.BODEventId).HasColumnName("bod_event_id");
            entity.Property(e => e.EquipmentName).HasColumnName("equipment_name");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
        });

        modelBuilder.Entity<BODTransportation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("bod_event_transportation");
            entity.Property(e => e.Id).HasColumnName("transportation_id");
            entity.Property(e => e.BODEventId).HasColumnName("bod_event_id");
            entity.Property(e => e.VehicleType).HasColumnName("vehicle_type");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
        });

        modelBuilder.Entity<BODExpense>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("bod_event_expense");
            entity.Property(e => e.Id).HasColumnName("expense_id");
            entity.Property(e => e.BODEventId).HasColumnName("bod_event_id");
            entity.Property(e => e.ItemName).HasColumnName("item_name");
            entity.Property(e => e.EstimatedCost).HasColumnName("estimated_cost").HasColumnType("decimal(15,2)");
        });

        // Leadership and Org
        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.ProfileId);
            entity.ToTable("profile");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.MiddleName).HasColumnName("middle_name");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.ContactNumber).HasColumnName("contact_number");
            entity.Property(e => e.Sex).HasColumnName("sex");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.ProfileStatus).HasColumnName("profile_status");
            entity.Property(e => e.ChurchId).HasColumnName("church_id");

            entity.HasOne(e => e.Account).WithOne().HasForeignKey<Account>(a => a.ProfileId);
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId);
            entity.ToTable("account");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.ChurchId).HasColumnName("church_id");
            entity.Property(e => e.DistrictId).HasColumnName("district_id");
            entity.Property(e => e.Username).HasColumnName("username");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId);
            entity.ToTable("role");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName).HasColumnName("role_name");
        });

        modelBuilder.Entity<LeadershipAssignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId);
            entity.ToTable("leadership_assignment");
            entity.Property(e => e.AssignmentId).HasColumnName("assignment_id");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.MinistryId).HasColumnName("ministry_id");
            entity.Property(e => e.DistrictId).HasColumnName("district_id");
            entity.Property(e => e.ChurchId).HasColumnName("church_id");
            entity.Property(e => e.AssignedDate).HasColumnName("assigned_date");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Profile).WithMany(p => p.LeadershipAssignments).HasForeignKey(d => d.ProfileId);
            entity.HasOne(d => d.Role).WithMany().HasForeignKey(d => d.RoleId);
            entity.HasOne(d => d.District).WithMany().HasForeignKey(d => d.DistrictId);
            entity.HasOne(d => d.Church).WithMany().HasForeignKey(d => d.ChurchId);
            entity.HasOne(d => d.Ministry).WithMany().HasForeignKey(d => d.MinistryId);
        });

        modelBuilder.Entity<Church>(entity =>
        {
            entity.HasKey(e => e.ChurchId);
            entity.ToTable("church");
            entity.Property(e => e.ChurchId).HasColumnName("church_id");
            entity.Property(e => e.ChurchName).HasColumnName("church_name");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.DistrictId).HasColumnName("district_id");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.DistrictId);
            entity.ToTable("district");
            entity.Property(e => e.DistrictId).HasColumnName("district_id");
            entity.Property(e => e.DistrictName).HasColumnName("district_name");
            entity.Property(e => e.DistrictCode).HasColumnName("district_code");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Ministry>(entity =>
        {
            entity.HasKey(e => e.MinistryId);
            entity.ToTable("ministry");
            entity.Property(e => e.MinistryId).HasColumnName("ministry_id");
            entity.Property(e => e.MinistryName).HasColumnName("ministry_name");
            entity.Property(e => e.Status).HasColumnName("status");
        });
    }
}
