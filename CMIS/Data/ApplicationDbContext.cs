using System;
using System.Collections.Generic;
using System.Text.Json;
using CMIS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CMIS.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Church> Churches { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<LeadershipAssignment> LeadershipAssignments { get; set; }

    public virtual DbSet<Ministry> Ministries { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<ProfileMinistry> ProfileMinistries { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<BudgetProposal> BudgetProposals { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<BudgetAllocation> BudgetAllocations { get; set; }

    public virtual DbSet<Budget> Budgets { get; set; }

    public virtual DbSet<Fund> Funds { get; set; }

    public virtual DbSet<EventModel> Events { get; set; }
    
    public virtual DbSet<ProgramScheduleItem> ProgramSchedules { get; set; }

    public virtual DbSet<GuestItem> Guests { get; set; }

    public virtual DbSet<PersonnelItem> Personnel { get; set; }

    public virtual DbSet<EquipmentItem> Equipment { get; set; }

    public virtual DbSet<TransportationItem> Transportation { get; set; }
    
    public virtual DbSet<ExpenseItem> Expenses { get; set; }

    public virtual DbSet<DistrictEventModel> DistrictEvents { get; set; }
    public virtual DbSet<DistrictProgramScheduleItem> DistrictProgramSchedules { get; set; }
    public virtual DbSet<DistrictGuestItem> DistrictGuests { get; set; }
    public virtual DbSet<DistrictPersonnelItem> DistrictPersonnel { get; set; }
    public virtual DbSet<DistrictEquipmentItem> DistrictEquipment { get; set; }
    public virtual DbSet<DistrictTransportationItem> DistrictTransportation { get; set; }
    public virtual DbSet<DistrictExpenseItem> DistrictExpenses { get; set; }
    
    public virtual DbSet<BODEventModel> BODEvents { get; set; }
    public virtual DbSet<BODProgramScheduleItem> BODProgramSchedules { get; set; }
    public virtual DbSet<BODGuestItem> BODGuests { get; set; }
    public virtual DbSet<BODPersonnelItem> BODPersonnel { get; set; }
    public virtual DbSet<BODEquipmentItem> BODEquipment { get; set; }
    public virtual DbSet<BODTransportationItem> BODTransportation { get; set; }
    public virtual DbSet<BODExpenseItem> BODExpenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PRIMARY");

            entity.ToTable("account");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.RoleId, "fk_account_role");

            entity.HasIndex(e => e.ProfileId, "profile_id").IsUnique();

            entity.HasIndex(e => e.Username, "username").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.ChurchId).HasColumnName("church_id");
            entity.Property(e => e.DistrictId).HasColumnName("district_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'Active'")
                .HasColumnType("enum('Active','Inactive')")
                .HasColumnName("status");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.Church).WithMany()
                .HasForeignKey(d => d.ChurchId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_account_church");

            entity.HasOne(d => d.District).WithMany()
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_account_district");

            entity.HasOne(d => d.Profile).WithOne(p => p.Account)
                .HasForeignKey<Account>(d => d.ProfileId)
                .HasConstraintName("fk_account_profile");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_account_role");
        });

        modelBuilder.Entity<Church>(entity =>
        {
            entity.HasKey(e => e.ChurchId).HasName("PRIMARY");

            entity.ToTable("church");

            entity.HasIndex(e => e.DistrictId, "fk_church_district");

            entity.Property(e => e.ChurchId).HasColumnName("church_id");
            entity.Property(e => e.Address)
                .HasColumnType("text")
                .HasColumnName("address");
            entity.Property(e => e.ChurchName)
                .HasMaxLength(150)
                .HasColumnName("church_name");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(20)
                .HasColumnName("contact_number");
            entity.Property(e => e.DistrictId).HasColumnName("district_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'Active'")
                .HasColumnType("enum('Active','Inactive')")
                .HasColumnName("status");
            entity.Property(e => e.FundId).HasColumnName("fund_id");

            entity.HasOne(d => d.District).WithMany(p => p.Churches)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_church_district");
            entity.HasOne(d => d.Fund).WithMany()
                .HasForeignKey(d => d.FundId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_church_fund");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.DistrictId).HasName("PRIMARY");

            entity.ToTable("district");

            entity.HasIndex(e => e.DistrictCode, "district_code").IsUnique();

            entity.Property(e => e.DistrictId).HasColumnName("district_id");
            entity.Property(e => e.Address)
                .HasColumnType("text")
                .HasColumnName("address");
            entity.Property(e => e.DistrictCode)
                .HasMaxLength(50)
                .HasColumnName("district_code");
            entity.Property(e => e.DistrictName)
                .HasMaxLength(150)
                .HasColumnName("district_name");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'Active'")
                .HasColumnType("enum('Active','Inactive')")
                .HasColumnName("status");
            entity.Property(e => e.FundId).HasColumnName("fund_id");
            entity.HasOne(d => d.Fund).WithMany()
                .HasForeignKey(d => d.FundId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_district_fund");
        });

        modelBuilder.Entity<LeadershipAssignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("PRIMARY");

            entity.ToTable("leadership_assignment");

            entity.HasIndex(e => e.ChurchId, "fk_leadership_church");

            entity.HasIndex(e => e.DistrictId, "fk_leadership_district");

            entity.HasIndex(e => e.MinistryId, "fk_leadership_ministry");

            entity.HasIndex(e => e.ProfileId, "fk_leadership_profile");

            entity.HasIndex(e => e.RoleId, "fk_leadership_role");

            entity.Property(e => e.AssignmentId).HasColumnName("assignment_id");
            entity.Property(e => e.AssignedDate).HasColumnName("assigned_date");
            entity.Property(e => e.ChurchId).HasColumnName("church_id");
            entity.Property(e => e.DistrictId).HasColumnName("district_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.MinistryId).HasColumnName("ministry_id");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'Active'")
                .HasColumnType("enum('Active','Inactive')")
                .HasColumnName("status");

            entity.HasOne(d => d.Church).WithMany(p => p.LeadershipAssignments)
                .HasForeignKey(d => d.ChurchId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_leadership_church");

            entity.HasOne(d => d.District).WithMany(p => p.LeadershipAssignments)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_leadership_district");

            entity.HasOne(d => d.Ministry).WithMany(p => p.LeadershipAssignments)
                .HasForeignKey(d => d.MinistryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_leadership_ministry");

            entity.HasOne(d => d.Profile).WithMany(p => p.LeadershipAssignments)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("fk_leadership_profile");

            entity.HasOne(d => d.Role).WithMany(p => p.LeadershipAssignments)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_leadership_role");
        });

        modelBuilder.Entity<Ministry>(entity =>
        {
            entity.HasKey(e => e.MinistryId).HasName("PRIMARY");

            entity.ToTable("ministry");

            entity.HasIndex(e => e.MinistryName, "ministry_name").IsUnique();

            entity.Property(e => e.MinistryId).HasColumnName("ministry_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.MinistryName)
                .HasMaxLength(150)
                .HasColumnName("ministry_name");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'Active'")
                .HasColumnType("enum('Active','Inactive')")
                .HasColumnName("status");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PRIMARY");

            entity.ToTable("profile");

            entity.HasIndex(e => e.ChurchId, "fk_profile_church");

            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.Address)
                .HasColumnType("text")
                .HasColumnName("address");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.ChurchId).HasColumnName("church_id");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(20)
                .HasColumnName("contact_number");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .HasColumnName("middle_name");
            entity.Property(e => e.ProfileStatus)
                .HasDefaultValueSql("'Active'")
                .HasColumnType("enum('Active','Inactive')")
                .HasColumnName("profile_status");
            entity.Property(e => e.Sex)
                .HasColumnType("enum('Male','Female')")
                .HasColumnName("sex");

            entity.HasOne(d => d.Church).WithMany(p => p.Profiles)
                .HasForeignKey(d => d.ChurchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_profile_church");
        });

        modelBuilder.Entity<ProfileMinistry>(entity =>
        {
            entity.HasKey(e => e.ProfileMinistryId).HasName("PRIMARY");

            entity.ToTable("profile_ministry");

            entity.HasIndex(e => e.MinistryId, "fk_profile_ministry_ministry");

            entity.HasIndex(e => e.ProfileId, "fk_profile_ministry_profile");

            entity.Property(e => e.ProfileMinistryId).HasColumnName("profile_ministry_id");
            entity.Property(e => e.JoinedAt).HasColumnName("joined_at");
            entity.Property(e => e.MinistryId).HasColumnName("ministry_id");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'Active'")
                .HasColumnType("enum('Active','Inactive')")
                .HasColumnName("status");

            entity.HasOne(d => d.Ministry).WithMany(p => p.ProfileMinistries)
                .HasForeignKey(d => d.MinistryId)
                .HasConstraintName("fk_profile_ministry_ministry");

            entity.HasOne(d => d.Profile).WithMany(p => p.ProfileMinistries)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("fk_profile_ministry_profile");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("role");

            entity.HasIndex(e => e.RoleName, "role_name").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<BudgetProposal>(entity =>
        {
            entity.HasKey(e => e.ProposalId).HasName("PRIMARY");

            entity.ToTable("budget_proposal");

            entity.HasIndex(e => e.ProposalCode, "proposal_code").IsUnique();

            entity.Property(e => e.ProposalId).HasColumnName("proposal_id");
            entity.Property(e => e.ProposalCode)
                .HasMaxLength(20)
                .HasColumnName("proposal_code");
            entity.Property(e => e.Purpose)
                .HasMaxLength(255)
                .HasColumnName("purpose");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.MinistryId).HasColumnName("ministry_id");
            entity.Property(e => e.ChurchId).HasColumnName("church_id");
            entity.Property(e => e.DistrictId).HasColumnName("district_id");
            entity.Property(e => e.Level)
                .HasMaxLength(20)
                .HasColumnName("level");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(15,2)")
                .HasColumnName("amount");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'Pending'")
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.RejectionReason).HasColumnType("text").HasColumnName("rejection_reason");
            entity.Property(e => e.SubmittedById).HasColumnName("submitted_by_id");
            entity.Property(e => e.ReviewedById).HasColumnName("reviewed_by_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Church).WithMany()
                .HasForeignKey(d => d.ChurchId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_proposal_church");

            entity.HasOne(d => d.District).WithMany()
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_proposal_district");

            entity.HasOne(d => d.Ministry).WithMany()
                .HasForeignKey(d => d.MinistryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_proposal_ministry");

            entity.HasOne(d => d.SubmittedBy).WithMany()
                .HasForeignKey(d => d.SubmittedById)
                .HasConstraintName("fk_proposal_submitted_by");

            entity.HasOne(d => d.ReviewedBy).WithMany()
                .HasForeignKey(d => d.ReviewedById)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_proposal_reviewed_by");
        });
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PRIMARY");
            entity.ToTable("transaction");
            entity.HasIndex(e => e.TransactionCode, "transaction_code").IsUnique();
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.TransactionCode).HasMaxLength(20).HasColumnName("transaction_code");
            entity.Property(e => e.Description).HasMaxLength(255).HasColumnName("description");
            entity.Property(e => e.Type).HasMaxLength(20).HasColumnName("type");
            entity.Property(e => e.BudgetAllocationId).HasColumnName("budget_allocation_id");
            entity.Property(e => e.BudgetLabel).HasMaxLength(150).HasColumnName("budget_label");
            entity.Property(e => e.Amount).HasColumnType("decimal(15,2)").HasColumnName("amount");
            entity.Property(e => e.RecordedById).HasColumnName("recorded_by_id");
            entity.Property(e => e.TransactionDate).HasColumnName("transaction_date");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnType("timestamp").HasColumnName("created_at");
            entity.HasOne(d => d.RecordedBy).WithMany().HasForeignKey(d => d.RecordedById).HasConstraintName("fk_transaction_recorded_by");
            entity.HasOne(d => d.Allocation).WithMany()
                .HasForeignKey(d => d.BudgetAllocationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_transaction_allocation");
        });

        modelBuilder.Entity<BudgetAllocation>(entity =>
        {
            entity.HasKey(e => e.AllocationId).HasName("PRIMARY");
            entity.ToTable("budget_allocation");
            entity.Property(e => e.AllocationId).HasColumnName("allocation_id");
            entity.Property(e => e.Name).HasMaxLength(150).HasColumnName("name");
            entity.Property(e => e.Category).HasMaxLength(50).HasColumnName("category");
            entity.Property(e => e.Allocated).HasColumnType("decimal(15,2)").HasColumnName("allocated");
            entity.Property(e => e.Spent).HasColumnType("decimal(15,2)").HasColumnName("spent");
            entity.Property(e => e.BudgetId).HasColumnName("budget_id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnType("timestamp").HasColumnName("created_at");
            entity.Ignore(e => e.RemainingBalance);
            entity.Ignore(e => e.Utilization);
            entity.HasOne(d => d.Budget).WithMany(b => b.Allocations)
                .HasForeignKey(d => d.BudgetId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_allocation_budget");
        });

        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.BudgetId).HasName("PRIMARY");
            entity.ToTable("budget");
            entity.Property(e => e.BudgetId).HasColumnName("budget_id");
            entity.Property(e => e.Name).HasMaxLength(150).HasColumnName("name");
            entity.Property(e => e.Level).HasMaxLength(50).HasColumnName("level");
            entity.Property(e => e.FiscalYear).HasMaxLength(10).HasColumnName("fiscal_year");
            entity.Property(e => e.ChurchId).HasColumnName("church_id");
            entity.Property(e => e.DistrictId).HasColumnName("district_id");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(15,2)").HasColumnName("total_amount");
            entity.Property(e => e.FundId).HasColumnName("fund_id");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnType("timestamp").HasColumnName("created_at");
            entity.HasOne(d => d.Church).WithMany().HasForeignKey(d => d.ChurchId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_budget_church");
            entity.HasOne(d => d.District).WithMany().HasForeignKey(d => d.DistrictId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_budget_district");
            entity.HasOne(d => d.Fund).WithMany().HasForeignKey(d => d.FundId).OnDelete(DeleteBehavior.SetNull).HasConstraintName("fk_budget_fund");
        });

        modelBuilder.Entity<Fund>(entity =>
        {
            entity.HasKey(e => e.FundId).HasName("PRIMARY");
            entity.ToTable("fund");
            entity.Property(e => e.FundId).HasColumnName("fund_id");
            entity.Property(e => e.Name).HasMaxLength(150).HasColumnName("name");
            entity.Property(e => e.Description).HasColumnType("text").HasColumnName("description");
            entity.Property(e => e.Level).HasMaxLength(50).HasColumnName("level");
            entity.Property(e => e.ChurchId).HasColumnName("church_id");
            entity.Property(e => e.DistrictId).HasColumnName("district_id");
            entity.Property(e => e.Amount).HasColumnType("decimal(15,2)").HasColumnName("amount");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnType("timestamp").HasColumnName("created_at");
            entity.HasOne(d => d.Church).WithMany()
                .HasForeignKey(d => d.ChurchId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_fund_church");
            entity.HasOne(d => d.District).WithMany()
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_fund_district");
        });

        modelBuilder.Entity<EventModel>(entity =>
        {
        entity.HasKey(e => e.Id).HasName("PRIMARY");
        entity.ToTable("event");

        entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
        entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(150);
        entity.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
        entity.Property(e => e.Venue).HasColumnName("venue").HasMaxLength(150);
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
        
        entity.Property(e => e.FundingType).HasColumnName("funding_type");
        entity.Property(e => e.ExpectedNumberOfParticipants).HasColumnName("expected_number_of_participants").HasColumnType("decimal(15,2)");
        entity.Property(e => e.TicketedPrice).HasColumnName("ticketed_price").HasColumnType("decimal(15,2)");
        entity.Property(e => e.AllocatedBudgetPerPerson).HasColumnName("allocated_budget_per_person").HasColumnType("decimal(15,2)");
        entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

        // Report Fields
        entity.Property(e => e.AttendanceCount).HasColumnName("attendance_count");
        entity.Property(e => e.WalkInCount).HasColumnName("walk_in_count");
        entity.Property(e => e.NewMembersCount).HasColumnName("new_members_count");
        entity.Property(e => e.ReportNotes).HasColumnName("report_notes").HasColumnType("text");
        entity.Property(e => e.IsRescheduled).HasColumnName("is_rescheduled");
        entity.Property(e => e.IsReportGenerated).HasColumnName("is_report_generated");

        // Setup Relationships (One-to-Many)
        entity.HasMany(e => e.EventProgramSchedules).WithOne().HasForeignKey(p => p.EventId);
        entity.HasMany(e => e.EventGuests).WithOne().HasForeignKey(g => g.EventId);
        entity.HasMany(e => e.EventPersonnel).WithOne().HasForeignKey(p => p.EventId);
        entity.HasMany(e => e.EventEquipments).WithOne().HasForeignKey(e => e.EventId);
        entity.HasMany(e => e.EventTransportations).WithOne().HasForeignKey(t => t.EventId);
        entity.HasMany(e => e.EventExpenses).WithOne().HasForeignKey(ex => ex.EventId);
        });

    // 2. Child Table Mappings (Matching your Data Dictionary Names)
        modelBuilder.Entity<ProgramScheduleItem>(entity => 
        {
        entity.ToTable("event_program_schedule");
        entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
        entity.Property(e => e.EventId).HasColumnName("event_id");
        entity.Property(e => e.StartTime).HasColumnName("start_time");
        entity.Property(e => e.EndTime).HasColumnName("end_time");
        entity.Property(e => e.ProgramTitle).HasColumnName("program_title");
        entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
        });

        modelBuilder.Entity<GuestItem>(entity => 
        {
        entity.ToTable("event_guest");
        entity.Property(e => e.GuestId).HasColumnName("guest_id");
        entity.Property(e => e.EventId).HasColumnName("event_id");
        entity.Property(e => e.GuestType).HasColumnName("guest_type");
        entity.Property(e => e.FullName).HasColumnName("full_name");
        entity.Property(e => e.ContactNumber).HasColumnName("contact_number");
        });

        modelBuilder.Entity<PersonnelItem>(entity => 
        {
        entity.ToTable("event_personnel");
        entity.Property(e => e.PersonnelId).HasColumnName("personnel_id");
        entity.Property(e => e.EventId).HasColumnName("event_id");
        entity.Property(e => e.RoleName).HasColumnName("role_name");
        entity.Property(e => e.FullName).HasColumnName("full_name");
        entity.Property(e => e.ContactNumber).HasColumnName("contact_number");
        });

        modelBuilder.Entity<EquipmentItem>(entity => 
        {
        entity.ToTable("event_equipment");
        entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
        entity.Property(e => e.EventId).HasColumnName("event_id");
        entity.Property(e => e.EquipmentName).HasColumnName("equipment_name");
        entity.Property(e => e.Remarks).HasColumnName("remarks");
        });

        modelBuilder.Entity<TransportationItem>(entity => 
        {
        entity.ToTable("event_transportation");
        entity.Property(e => e.TransportationId).HasColumnName("transportation_id");
        entity.Property(e => e.EventId).HasColumnName("event_id");
        entity.Property(e => e.VehicleType).HasColumnName("vehicle_type");
        entity.Property(e => e.Remarks).HasColumnName("remarks");
        });

        modelBuilder.Entity<ExpenseItem>(entity => 
        {
        entity.ToTable("event_expense");
        entity.Property(e => e.ExpenseId).HasColumnName("expense_id");
        entity.Property(e => e.EventId).HasColumnName("financial_id"); 
        entity.Property(e => e.ItemName).HasColumnName("item_name");
        entity.Property(e => e.EstimatedCost).HasColumnName("estimated_cost").HasColumnType("decimal(15,2)");
        });
        // modelBuilder.Entity<EventModel>(entity =>
        // {
        //     entity.HasKey(e => e.Id).HasName("PRIMARY");
        //     entity.ToTable("event");

        //     entity.Property(e => e.Id).HasColumnName("id");
        //     entity.Property(e => e.Title).HasColumnName("title");
        //     entity.Property(e => e.Description).HasColumnType("text").HasColumnName("description");
        //     entity.Property(e => e.Venue).HasColumnName("venue");
        //     entity.Property(e => e.Date).HasColumnName("date");
        //     entity.Property(e => e.Category).HasColumnName("category");
        //     entity.Property(e => e.ImageUrl).HasColumnType("text").HasColumnName("image_url");
        //     entity.Property(e => e.StartTime).HasColumnName("start_time");
        //     entity.Property(e => e.EndTime).HasColumnName("end_time");
        //     entity.Property(e => e.Duration).HasColumnName("duration");
        //     entity.Property(e => e.ExpectedParticipants).HasColumnName("expected_participants");
        //     entity.Property(e => e.TargetAudience).HasColumnName("target_audience");
        //     entity.Property(e => e.OrganizingMinistry).HasColumnName("organizing_ministry");
        //     entity.Property(e => e.Status).HasColumnName("status");
            
        //     entity.Property(e => e.FundingType).HasColumnName("funding_type");
        //     entity.Property(e => e.ExpectedNumberOfParticipants).HasColumnType("decimal(15,2)").HasColumnName("expected_number_of_participants");
        //     entity.Property(e => e.TicketedPrice).HasColumnType("decimal(15,2)").HasColumnName("ticketed_price");
        //     entity.Property(e => e.AllocatedBudgetPerPerson).HasColumnType("decimal(15,2)").HasColumnName("allocated_budget_per_person");


            // var jsonOptions = (JsonSerializerOptions)null;

            // var programScheduleComparer = new ValueComparer<List<ProgramScheduleItem>>(
            //     (c1, c2) => JsonSerializer.Serialize(c1, jsonOptions) == JsonSerializer.Serialize(c2, jsonOptions),
            //     c => c == null ? 0 : JsonSerializer.Serialize(c, jsonOptions).GetHashCode(),
            //     c => JsonSerializer.Deserialize<List<ProgramScheduleItem>>(JsonSerializer.Serialize(c, jsonOptions), jsonOptions)
            // );

            // entity.Property(e => e.ProgramSchedules)
            //     .HasConversion(
            //         v => JsonSerializer.Serialize(v, jsonOptions),
            //         v => JsonSerializer.Deserialize<List<ProgramScheduleItem>>(v, jsonOptions))
            //     .Metadata.SetValueComparer(programScheduleComparer);
            // entity.Property(e => e.ProgramSchedules).HasColumnType("json");

            // var guestComparer = new ValueComparer<List<GuestItem>>(
            //     (c1, c2) => JsonSerializer.Serialize(c1, jsonOptions) == JsonSerializer.Serialize(c2, jsonOptions),
            //     c => c == null ? 0 : JsonSerializer.Serialize(c, jsonOptions).GetHashCode(),
            //     c => JsonSerializer.Deserialize<List<GuestItem>>(JsonSerializer.Serialize(c, jsonOptions), jsonOptions)
            // );

            // entity.Property(e => e.Guests)
            //     .HasConversion(
            //         v => JsonSerializer.Serialize(v, jsonOptions),
            //         v => JsonSerializer.Deserialize<List<GuestItem>>(v, jsonOptions))
            //     .Metadata.SetValueComparer(guestComparer);
            // entity.Property(e => e.Guests).HasColumnType("json");

            // var personnelComparer = new ValueComparer<List<PersonnelItem>>(
            //     (c1, c2) => JsonSerializer.Serialize(c1, jsonOptions) == JsonSerializer.Serialize(c2, jsonOptions),
            //     c => c == null ? 0 : JsonSerializer.Serialize(c, jsonOptions).GetHashCode(),
            //     c => JsonSerializer.Deserialize<List<PersonnelItem>>(JsonSerializer.Serialize(c, jsonOptions), jsonOptions)
            // );

            // entity.Property(e => e.Personnel)
            //     .HasConversion(
            //         v => JsonSerializer.Serialize(v, jsonOptions),
            //         v => JsonSerializer.Deserialize<List<PersonnelItem>>(v, jsonOptions))
            //     .Metadata.SetValueComparer(personnelComparer);
            // entity.Property(e => e.Personnel).HasColumnType("json");

            // var equipmentComparer = new ValueComparer<List<EquipmentItem>>(
            //     (c1, c2) => JsonSerializer.Serialize(c1, jsonOptions) == JsonSerializer.Serialize(c2, jsonOptions),
            //     c => c == null ? 0 : JsonSerializer.Serialize(c, jsonOptions).GetHashCode(),
            //     c => JsonSerializer.Deserialize<List<EquipmentItem>>(JsonSerializer.Serialize(c, jsonOptions), jsonOptions)
            // );

            // entity.Property(e => e.Equipment)
            //     .HasConversion(
            //         v => JsonSerializer.Serialize(v, jsonOptions),
            //         v => JsonSerializer.Deserialize<List<EquipmentItem>>(v, jsonOptions))
            //     .Metadata.SetValueComparer(equipmentComparer);
            // entity.Property(e => e.Equipment).HasColumnType("json");

            // var transportationComparer = new ValueComparer<List<TransportationItem>>(
            //     (c1, c2) => JsonSerializer.Serialize(c1, jsonOptions) == JsonSerializer.Serialize(c2, jsonOptions),
            //     c => c == null ? 0 : JsonSerializer.Serialize(c, jsonOptions).GetHashCode(),
            //     c => JsonSerializer.Deserialize<List<TransportationItem>>(JsonSerializer.Serialize(c, jsonOptions), jsonOptions)
            // );

            // entity.Property(e => e.Transportation)
            //     .HasConversion(
            //         v => JsonSerializer.Serialize(v, jsonOptions),
            //         v => JsonSerializer.Deserialize<List<TransportationItem>>(v, jsonOptions))
            //     .Metadata.SetValueComparer(transportationComparer);
            // entity.Property(e => e.Transportation).HasColumnType("json");

            // var expenseComparer = new ValueComparer<List<ExpenseItem>>(
            //     (c1, c2) => JsonSerializer.Serialize(c1, jsonOptions) == JsonSerializer.Serialize(c2, jsonOptions),
            //     c => c == null ? 0 : JsonSerializer.Serialize(c, jsonOptions).GetHashCode(),
            //     c => JsonSerializer.Deserialize<List<ExpenseItem>>(JsonSerializer.Serialize(c, jsonOptions), jsonOptions)
            // );

            // entity.Property(e => e.Expenses)
            //     .HasConversion(
            //         v => JsonSerializer.Serialize(v, jsonOptions),
            //         v => JsonSerializer.Deserialize<List<ExpenseItem>>(v, jsonOptions))
            //     .Metadata.SetValueComparer(expenseComparer);
            // entity.Property(e => e.Expenses).HasColumnType("json");
        

        // District Event Table Mappings
        modelBuilder.Entity<DistrictEventModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("district_event");

            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(150);
            entity.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
            entity.Property(e => e.Venue).HasColumnName("venue").HasMaxLength(150);
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
            
            entity.Property(e => e.FundingType).HasColumnName("funding_type");
            entity.Property(e => e.ExpectedNumberOfParticipants).HasColumnName("expected_number_of_participants").HasColumnType("decimal(15,2)");
            entity.Property(e => e.TicketedPrice).HasColumnName("ticketed_price").HasColumnType("decimal(15,2)");
            entity.Property(e => e.AllocatedBudgetPerPerson).HasColumnName("allocated_budget_per_person").HasColumnType("decimal(15,2)");
            
            entity.Property(e => e.AssignedLeaderId).HasColumnName("assigned_leader_id");
            entity.Property(e => e.AssignedLeaderName).HasColumnName("assigned_leader_name");
            
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

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

        modelBuilder.Entity<DistrictProgramScheduleItem>(entity => 
        {
            entity.ToTable("district_event_program_schedule");
            entity.Property(e => e.Id).HasColumnName("schedule_id");
            entity.Property(e => e.DistrictEventId).HasColumnName("district_event_id");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.ProgramTitle).HasColumnName("program_title");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
        });

        modelBuilder.Entity<DistrictGuestItem>(entity => 
        {
            entity.ToTable("district_event_guest");
            entity.Property(e => e.Id).HasColumnName("guest_id");
            entity.Property(e => e.DistrictEventId).HasColumnName("district_event_id");
            entity.Property(e => e.GuestType).HasColumnName("guest_type");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.ContactNumber).HasColumnName("contact_number");
        });

        modelBuilder.Entity<DistrictPersonnelItem>(entity => 
        {
            entity.ToTable("district_event_personnel");
            entity.Property(e => e.Id).HasColumnName("personnel_id");
            entity.Property(e => e.DistrictEventId).HasColumnName("district_event_id");
            entity.Property(e => e.RoleName).HasColumnName("role_name");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.ContactNumber).HasColumnName("contact_number");
        });

        modelBuilder.Entity<DistrictEquipmentItem>(entity => 
        {
            entity.ToTable("district_event_equipment");
            entity.Property(e => e.Id).HasColumnName("equipment_id");
            entity.Property(e => e.DistrictEventId).HasColumnName("district_event_id");
            entity.Property(e => e.EquipmentName).HasColumnName("equipment_name");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
        });

        modelBuilder.Entity<DistrictTransportationItem>(entity => 
        {
            entity.ToTable("district_event_transportation");
            entity.Property(e => e.Id).HasColumnName("transportation_id");
            entity.Property(e => e.DistrictEventId).HasColumnName("district_event_id");
            entity.Property(e => e.VehicleType).HasColumnName("vehicle_type");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
        });

        modelBuilder.Entity<DistrictExpenseItem>(entity => 
        {
            entity.ToTable("district_event_expense");
            entity.Property(e => e.Id).HasColumnName("expense_id");
            entity.Property(e => e.DistrictEventId).HasColumnName("district_event_id"); 
            entity.Property(e => e.ItemName).HasColumnName("item_name");
            entity.Property(e => e.EstimatedCost).HasColumnName("estimated_cost").HasColumnType("decimal(15,2)");
        });

        // BOD Event Table Mappings
        modelBuilder.Entity<BODEventModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("bod_event");

            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(150);
            entity.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
            entity.Property(e => e.Venue).HasColumnName("venue").HasMaxLength(150);
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
            
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

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

        modelBuilder.Entity<BODProgramScheduleItem>(entity => 
        {
            entity.ToTable("bod_event_program_schedule");
            entity.Property(e => e.Id).HasColumnName("schedule_id");
            entity.Property(e => e.BODEventId).HasColumnName("bod_event_id");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.ProgramTitle).HasColumnName("program_title");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
        });

        modelBuilder.Entity<BODGuestItem>(entity => 
        {
            entity.ToTable("bod_event_guest");
            entity.Property(e => e.Id).HasColumnName("guest_id");
            entity.Property(e => e.BODEventId).HasColumnName("bod_event_id");
            entity.Property(e => e.GuestType).HasColumnName("guest_type");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.ContactNumber).HasColumnName("contact_number");
        });

        modelBuilder.Entity<BODPersonnelItem>(entity => 
        {
            entity.ToTable("bod_event_personnel");
            entity.Property(e => e.Id).HasColumnName("personnel_id");
            entity.Property(e => e.BODEventId).HasColumnName("bod_event_id");
            entity.Property(e => e.RoleName).HasColumnName("role_name");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.ContactNumber).HasColumnName("contact_number");
        });

        modelBuilder.Entity<BODEquipmentItem>(entity => 
        {
            entity.ToTable("bod_event_equipment");
            entity.Property(e => e.Id).HasColumnName("equipment_id");
            entity.Property(e => e.BODEventId).HasColumnName("bod_event_id");
            entity.Property(e => e.EquipmentName).HasColumnName("equipment_name");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
        });

        modelBuilder.Entity<BODTransportationItem>(entity => 
        {
            entity.ToTable("bod_event_transportation");
            entity.Property(e => e.Id).HasColumnName("transportation_id");
            entity.Property(e => e.BODEventId).HasColumnName("bod_event_id");
            entity.Property(e => e.VehicleType).HasColumnName("vehicle_type");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
        });

        modelBuilder.Entity<BODExpenseItem>(entity => 
        {
            entity.ToTable("bod_event_expense");
            entity.Property(e => e.Id).HasColumnName("expense_id");
            entity.Property(e => e.BODEventId).HasColumnName("bod_event_id"); 
            entity.Property(e => e.ItemName).HasColumnName("item_name");
            entity.Property(e => e.EstimatedCost).HasColumnName("estimated_cost").HasColumnType("decimal(15,2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
