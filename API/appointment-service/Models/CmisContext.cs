using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace appointment_service.Models;

public partial class CmisContext : DbContext
{
    public CmisContext()
    {
    }

    public CmisContext(DbContextOptions<CmisContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Baptism> Baptisms { get; set; }

    public virtual DbSet<BudgetAllocation> BudgetAllocations { get; set; }

    public virtual DbSet<BudgetProposal> BudgetProposals { get; set; }

    public virtual DbSet<ChildDedication> ChildDedications { get; set; }

    public virtual DbSet<ChildDedicationWitness> ChildDedicationWitnesses { get; set; }

    public virtual DbSet<Church> Churches { get; set; }

    public virtual DbSet<Counsel> Counsels { get; set; }

    public virtual DbSet<DayException> DayExceptions { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventEquipment> EventEquipments { get; set; }

    public virtual DbSet<EventExpense> EventExpenses { get; set; }

    public virtual DbSet<EventGuest> EventGuests { get; set; }

    public virtual DbSet<EventPersonnel> EventPersonnel { get; set; }

    public virtual DbSet<EventProgramSchedule> EventProgramSchedules { get; set; }

    public virtual DbSet<EventTransportation> EventTransportations { get; set; }

    public virtual DbSet<Funeral> Funerals { get; set; }

    public virtual DbSet<LeadershipAssignment> LeadershipAssignments { get; set; }

    public virtual DbSet<Ministry> Ministries { get; set; }

    public virtual DbSet<ParentProfile> ParentProfiles { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<ProfileMinistry> ProfileMinistries { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TimeException> TimeExceptions { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Wedding> Weddings { get; set; }

    public virtual DbSet<WeddingProfile> WeddingProfiles { get; set; }

    public virtual DbSet<WeddingWitness> WeddingWitnesses { get; set; }

    public virtual DbSet<WeeklyConfig> WeeklyConfigs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("YOUR_PASSWORD_HERE");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'Active'")
                .HasColumnType("enum('Active','Inactive')")
                .HasColumnName("status");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.Profile).WithOne(p => p.Account)
                .HasForeignKey<Account>(d => d.ProfileId)
                .HasConstraintName("fk_account_profile");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_account_role");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PRIMARY");

            entity.ToTable("appointment");

            entity.HasIndex(e => e.AssignedToId, "fk_assigned_to_id_appointment");

            entity.HasIndex(e => e.RequesterId, "fk_requester_id_appointment");

            entity.HasIndex(e => new { e.StartTime, e.EndTime, e.Date, e.AssignedToId }, "unique_schedule").IsUnique();

            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.AssignedToId).HasColumnName("assigned_to_id");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");
            entity.Property(e => e.EndTime)
                .HasColumnType("time")
                .HasColumnName("end_time");
            entity.Property(e => e.RequesterId).HasColumnName("requester_id");
            entity.Property(e => e.ServiceType)
                .HasColumnType("enum('funeral','baptism','counsel','wedding','child dedication')")
                .HasColumnName("service_type");
            entity.Property(e => e.StartTime)
                .HasColumnType("time")
                .HasColumnName("start_time");
            entity.Property(e => e.Status)
                .HasColumnType("enum('pending','confirmed','rescheduled','cancelled','complete')")
                .HasColumnName("status");

            entity.HasOne(d => d.AssignedTo).WithMany(p => p.AppointmentAssignedTos)
                .HasForeignKey(d => d.AssignedToId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_assigned_to_id_appointment");

            entity.HasOne(d => d.Requester).WithMany(p => p.AppointmentRequesters)
                .HasForeignKey(d => d.RequesterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requester_id_appointment");
        });

        modelBuilder.Entity<Baptism>(entity =>
        {
            entity.HasKey(e => e.BaptismId).HasName("PRIMARY");

            entity.ToTable("baptism");

            entity.HasIndex(e => e.AppointmentId, "appointment_id").IsUnique();

            entity.HasIndex(e => e.AssignedToId, "fk_assigned_to_id_baptism");

            entity.HasIndex(e => e.ProfileId, "fk_profile_id_baptism");

            entity.HasIndex(e => e.RequesterId, "fk_requester_id_baptism");

            entity.Property(e => e.BaptismId).HasColumnName("baptism_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.AssignedToId).HasColumnName("assigned_to_id");
            entity.Property(e => e.EndTime)
                .HasColumnType("time")
                .HasColumnName("end_time");
            entity.Property(e => e.ImplementationDate)
                .HasColumnType("date")
                .HasColumnName("implementation_date");
            entity.Property(e => e.Occupation)
                .HasMaxLength(100)
                .HasColumnName("occupation");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.RequesterId).HasColumnName("requester_id");
            entity.Property(e => e.StartTime)
                .HasColumnType("time")
                .HasColumnName("start_time");
            entity.Property(e => e.Status)
                .HasColumnType("enum('in progress','scheduled','rescheduled','cancelled','complete')")
                .HasColumnName("status");
            entity.Property(e => e.Venue)
                .HasMaxLength(100)
                .HasColumnName("venue");

            entity.HasOne(d => d.Appointment).WithOne(p => p.Baptism)
                .HasForeignKey<Baptism>(d => d.AppointmentId)
                .HasConstraintName("fk_appointment_id_baptism");

            entity.HasOne(d => d.AssignedTo).WithMany(p => p.BaptismAssignedTos)
                .HasForeignKey(d => d.AssignedToId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_assigned_to_id_baptism");

            entity.HasOne(d => d.Profile).WithMany(p => p.Baptisms)
                .HasForeignKey(d => d.ProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_profile_id_baptism");

            entity.HasOne(d => d.Requester).WithMany(p => p.BaptismRequesters)
                .HasForeignKey(d => d.RequesterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requester_id_baptism");
        });

        modelBuilder.Entity<BudgetAllocation>(entity =>
        {
            entity.HasKey(e => e.AllocationId).HasName("PRIMARY");

            entity.ToTable("budget_allocation");

            entity.Property(e => e.AllocationId).HasColumnName("allocation_id");
            entity.Property(e => e.Allocated)
                .HasPrecision(15)
                .HasColumnName("allocated");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Spent)
                .HasPrecision(15)
                .HasColumnName("spent");
        });

        modelBuilder.Entity<BudgetProposal>(entity =>
        {
            entity.HasKey(e => e.ProposalId).HasName("PRIMARY");

            entity.ToTable("budget_proposal");

            entity.HasIndex(e => e.MinistryId, "IX_budget_proposal_ministry_id");

            entity.HasIndex(e => e.ReviewedById, "IX_budget_proposal_reviewed_by_id");

            entity.HasIndex(e => e.SubmittedById, "IX_budget_proposal_submitted_by_id");

            entity.HasIndex(e => e.ProposalCode, "proposal_code").IsUnique();

            entity.Property(e => e.ProposalId).HasColumnName("proposal_id");
            entity.Property(e => e.Amount)
                .HasPrecision(15)
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Level)
                .HasMaxLength(20)
                .HasColumnName("level");
            entity.Property(e => e.MinistryId).HasColumnName("ministry_id");
            entity.Property(e => e.ProposalCode)
                .HasMaxLength(20)
                .HasColumnName("proposal_code");
            entity.Property(e => e.Purpose)
                .HasMaxLength(255)
                .HasColumnName("purpose");
            entity.Property(e => e.ReviewedById).HasColumnName("reviewed_by_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Pending'")
                .HasColumnName("status");
            entity.Property(e => e.SubmittedById).HasColumnName("submitted_by_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Ministry).WithMany(p => p.BudgetProposals)
                .HasForeignKey(d => d.MinistryId)
                .HasConstraintName("fk_proposal_ministry");

            entity.HasOne(d => d.ReviewedBy).WithMany(p => p.BudgetProposalReviewedBies)
                .HasForeignKey(d => d.ReviewedById)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_proposal_reviewed_by");

            entity.HasOne(d => d.SubmittedBy).WithMany(p => p.BudgetProposalSubmittedBies)
                .HasForeignKey(d => d.SubmittedById)
                .HasConstraintName("fk_proposal_submitted_by");
        });

        modelBuilder.Entity<ChildDedication>(entity =>
        {
            entity.HasKey(e => e.ChildDedicationId).HasName("PRIMARY");

            entity.ToTable("child_dedication");

            entity.HasIndex(e => e.AppointmentId, "appointment_id").IsUnique();

            entity.HasIndex(e => e.CounselId, "counsel_id").IsUnique();

            entity.HasIndex(e => e.AssignedToId, "fk_assigned_to_id_child_dedication");

            entity.HasIndex(e => e.ChildProfileId, "fk_profile_id_child_dedication");

            entity.HasIndex(e => e.RequesterId, "fk_requester_id_child_dedication");

            entity.Property(e => e.ChildDedicationId).HasColumnName("child_dedication_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.AssignedToId).HasColumnName("assigned_to_id");
            entity.Property(e => e.BirthCert)
                .HasDefaultValueSql("'0'")
                .HasColumnName("birth_cert");
            entity.Property(e => e.ChildPlaceOfBirth)
                .HasMaxLength(100)
                .HasColumnName("child_place_of_birth");
            entity.Property(e => e.ChildProfileId).HasColumnName("child_profile_id");
            entity.Property(e => e.ChildWeightAtBirth)
                .HasPrecision(4)
                .HasColumnName("child_weight_at_birth");
            entity.Property(e => e.CounselId).HasColumnName("counsel_id");
            entity.Property(e => e.EndTime)
                .HasColumnType("time")
                .HasColumnName("end_time");
            entity.Property(e => e.ImplementationDate)
                .HasColumnType("date")
                .HasColumnName("implementation_date");
            entity.Property(e => e.ParentsMarriageDate)
                .HasColumnType("date")
                .HasColumnName("parents_marriage_date");
            entity.Property(e => e.ParentsMarriagePlaceCountry)
                .HasMaxLength(100)
                .HasColumnName("parents_marriage_place_country");
            entity.Property(e => e.ParentsMarriagePlaceMunicipality)
                .HasMaxLength(100)
                .HasColumnName("parents_marriage_place_municipality");
            entity.Property(e => e.ParentsMarriagePlaceProvince)
                .HasMaxLength(100)
                .HasColumnName("parents_marriage_place_province");
            entity.Property(e => e.RequesterId).HasColumnName("requester_id");
            entity.Property(e => e.StartTime)
                .HasColumnType("time")
                .HasColumnName("start_time");
            entity.Property(e => e.Status)
                .HasColumnType("enum('in progress','scheduled','rescheduled','cancelled','complete')")
                .HasColumnName("status");
            entity.Property(e => e.Venue)
                .HasMaxLength(100)
                .HasColumnName("venue");

            entity.HasOne(d => d.Appointment).WithOne(p => p.ChildDedication)
                .HasForeignKey<ChildDedication>(d => d.AppointmentId)
                .HasConstraintName("fk_appointment_id_child_dedication");

            entity.HasOne(d => d.AssignedTo).WithMany(p => p.ChildDedicationAssignedTos)
                .HasForeignKey(d => d.AssignedToId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_assigned_to_id_child_dedication");

            entity.HasOne(d => d.ChildProfile).WithMany(p => p.ChildDedications)
                .HasForeignKey(d => d.ChildProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_profile_id_child_dedication");

            entity.HasOne(d => d.Counsel).WithOne(p => p.ChildDedication)
                .HasForeignKey<ChildDedication>(d => d.CounselId)
                .HasConstraintName("fk_counsel_id_child_dedication");

            entity.HasOne(d => d.Requester).WithMany(p => p.ChildDedicationRequesters)
                .HasForeignKey(d => d.RequesterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requester_id_child_dedication");
        });

        modelBuilder.Entity<ChildDedicationWitness>(entity =>
        {
            entity.HasKey(e => e.ChildDedicationWitnessId).HasName("PRIMARY");

            entity.ToTable("child_dedication_witness");

            entity.HasIndex(e => e.ChildDedicationId, "fk_child_dedication_id_witness");

            entity.HasIndex(e => e.ProfileId, "fk_profile_id_child_dedication_witness");

            entity.Property(e => e.ChildDedicationWitnessId).HasColumnName("child_dedication_witness_id");
            entity.Property(e => e.ChildDedicationId).HasColumnName("child_dedication_id");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");

            entity.HasOne(d => d.ChildDedication).WithMany(p => p.ChildDedicationWitnesses)
                .HasForeignKey(d => d.ChildDedicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_child_dedication_id_witness");

            entity.HasOne(d => d.Profile).WithMany(p => p.ChildDedicationWitnesses)
                .HasForeignKey(d => d.ProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_profile_id_child_dedication_witness");
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

            entity.HasOne(d => d.District).WithMany(p => p.Churches)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_church_district");
        });

        modelBuilder.Entity<Counsel>(entity =>
        {
            entity.HasKey(e => e.CounselId).HasName("PRIMARY");

            entity.ToTable("counsel");

            entity.HasIndex(e => e.AppointmentId, "appointment_id").IsUnique();

            entity.HasIndex(e => e.AssignedToId, "fk_assigned_to_id_counsel");

            entity.HasIndex(e => e.ProfileId, "fk_profile_id_counsel");

            entity.HasIndex(e => e.RequesterId, "fk_requester_id_counsel");

            entity.Property(e => e.CounselId).HasColumnName("counsel_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.AssignedToId).HasColumnName("assigned_to_id");
            entity.Property(e => e.CounselFor)
                .HasMaxLength(100)
                .HasColumnName("counsel_for");
            entity.Property(e => e.EndTime)
                .HasColumnType("time")
                .HasColumnName("end_time");
            entity.Property(e => e.ImplementationDate)
                .HasColumnType("date")
                .HasColumnName("implementation_date");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.RequesterId).HasColumnName("requester_id");
            entity.Property(e => e.StartTime)
                .HasColumnType("time")
                .HasColumnName("start_time");
            entity.Property(e => e.Status)
                .HasColumnType("enum('in progress','scheduled','rescheduled','cancelled','complete')")
                .HasColumnName("status");

            entity.HasOne(d => d.Appointment).WithOne(p => p.Counsel)
                .HasForeignKey<Counsel>(d => d.AppointmentId)
                .HasConstraintName("fk_appointment_id_counsel");

            entity.HasOne(d => d.AssignedTo).WithMany(p => p.CounselAssignedTos)
                .HasForeignKey(d => d.AssignedToId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_assigned_to_id_counsel");

            entity.HasOne(d => d.Profile).WithMany(p => p.Counsels)
                .HasForeignKey(d => d.ProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_profile_id_counsel");

            entity.HasOne(d => d.Requester).WithMany(p => p.CounselRequesters)
                .HasForeignKey(d => d.RequesterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requester_id_counsel");
        });

        modelBuilder.Entity<DayException>(entity =>
        {
            entity.HasKey(e => e.DayExceptionId).HasName("PRIMARY");

            entity.ToTable("day_exception");

            entity.HasIndex(e => e.AccountId, "fk_account_id_day_exception");

            entity.HasIndex(e => new { e.Date, e.AccountId }, "unique_schedule_day_exception").IsUnique();

            entity.Property(e => e.DayExceptionId).HasColumnName("day_exception_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");

            entity.HasOne(d => d.Account).WithMany(p => p.DayExceptions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_account_id_day_exception");
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
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("event");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AllocatedBudgetPerPerson)
                .HasPrecision(15)
                .HasColumnName("allocated_budget_per_person");
            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .HasColumnName("category");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Duration)
                .HasMaxLength(100)
                .HasColumnName("duration");
            entity.Property(e => e.EndTime)
                .HasColumnType("time")
                .HasColumnName("end_time");
            entity.Property(e => e.ExpectedNumberOfParticipants)
                .HasPrecision(15)
                .HasColumnName("expected_number_of_participants");
            entity.Property(e => e.ExpectedParticipants).HasColumnName("expected_participants");
            entity.Property(e => e.FundingType)
                .HasMaxLength(150)
                .HasColumnName("funding_type");
            entity.Property(e => e.ImageUrl)
                .HasColumnType("text")
                .HasColumnName("image_url");
            entity.Property(e => e.OrganizingMinistry)
                .HasMaxLength(100)
                .HasColumnName("organizing_ministry");
            entity.Property(e => e.StartTime)
                .HasColumnType("time")
                .HasColumnName("start_time");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Under Review'")
                .HasColumnName("status");
            entity.Property(e => e.TargetAudience)
                .HasMaxLength(100)
                .HasColumnName("target_audience");
            entity.Property(e => e.TicketedPrice)
                .HasPrecision(15)
                .HasColumnName("ticketed_price");
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("updated_at");
            entity.Property(e => e.Venue)
                .HasMaxLength(150)
                .HasColumnName("venue");
        });

        modelBuilder.Entity<EventEquipment>(entity =>
        {
            entity.HasKey(e => e.EquipmentId).HasName("PRIMARY");

            entity.ToTable("event_equipment");

            entity.HasIndex(e => e.EventId, "fk_event_equipment");

            entity.Property(e => e.EquipmentId).HasColumnName("equipment_id");
            entity.Property(e => e.EquipmentName)
                .HasMaxLength(255)
                .HasColumnName("equipment_name");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.Remarks)
                .HasMaxLength(255)
                .HasColumnName("remarks");

            entity.HasOne(d => d.Event).WithMany(p => p.EventEquipments)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("fk_event_equipment");
        });

        modelBuilder.Entity<EventExpense>(entity =>
        {
            entity.HasKey(e => e.ExpenseId).HasName("PRIMARY");

            entity.ToTable("event_expense");

            entity.HasIndex(e => e.FinancialId, "fk_event_expense");

            entity.Property(e => e.ExpenseId).HasColumnName("expense_id");
            entity.Property(e => e.EstimatedCost)
                .HasPrecision(15)
                .HasColumnName("estimated_cost");
            entity.Property(e => e.FinancialId).HasColumnName("financial_id");
            entity.Property(e => e.ItemName)
                .HasMaxLength(255)
                .HasColumnName("item_name");

            entity.HasOne(d => d.Financial).WithMany(p => p.EventExpenses)
                .HasForeignKey(d => d.FinancialId)
                .HasConstraintName("fk_event_expense");
        });

        modelBuilder.Entity<EventGuest>(entity =>
        {
            entity.HasKey(e => e.GuestId).HasName("PRIMARY");

            entity.ToTable("event_guest");

            entity.HasIndex(e => e.EventId, "fk_event_guest");

            entity.Property(e => e.GuestId).HasColumnName("guest_id");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(50)
                .HasColumnName("contact_number");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.GuestType)
                .HasMaxLength(100)
                .HasColumnName("guest_type");

            entity.HasOne(d => d.Event).WithMany(p => p.EventGuests)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("fk_event_guest");
        });

        modelBuilder.Entity<EventPersonnel>(entity =>
        {
            entity.HasKey(e => e.PersonnelId).HasName("PRIMARY");

            entity.ToTable("event_personnel");

            entity.HasIndex(e => e.EventId, "fk_event_personnel");

            entity.Property(e => e.PersonnelId).HasColumnName("personnel_id");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(50)
                .HasColumnName("contact_number");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .HasColumnName("role_name");

            entity.HasOne(d => d.Event).WithMany(p => p.EventPersonnel)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("fk_event_personnel");
        });

        modelBuilder.Entity<EventProgramSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PRIMARY");

            entity.ToTable("event_program_schedule");

            entity.HasIndex(e => e.EventId, "fk_event_schedule");

            entity.Property(e => e.ScheduleId).HasColumnName("schedule_id");
            entity.Property(e => e.DisplayOrder).HasColumnName("display_order");
            entity.Property(e => e.EndTime)
                .HasColumnType("time")
                .HasColumnName("end_time");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.ProgramTitle)
                .HasMaxLength(255)
                .HasColumnName("program_title");
            entity.Property(e => e.StartTime)
                .HasColumnType("time")
                .HasColumnName("start_time");

            entity.HasOne(d => d.Event).WithMany(p => p.EventProgramSchedules)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("fk_event_schedule");
        });

        modelBuilder.Entity<EventTransportation>(entity =>
        {
            entity.HasKey(e => e.TransportationId).HasName("PRIMARY");

            entity.ToTable("event_transportation");

            entity.HasIndex(e => e.EventId, "fk_event_transportation");

            entity.Property(e => e.TransportationId).HasColumnName("transportation_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.Remarks)
                .HasMaxLength(255)
                .HasColumnName("remarks");
            entity.Property(e => e.VehicleType)
                .HasMaxLength(255)
                .HasColumnName("vehicle_type");

            entity.HasOne(d => d.Event).WithMany(p => p.EventTransportations)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("fk_event_transportation");
        });

        modelBuilder.Entity<Funeral>(entity =>
        {
            entity.HasKey(e => e.FuneralId).HasName("PRIMARY");

            entity.ToTable("funeral");

            entity.HasIndex(e => e.AppointmentId, "appointment_id").IsUnique();

            entity.HasIndex(e => e.AssignedToId, "fk_assigned_to_id_funeral");

            entity.HasIndex(e => e.DeceasedProfileId, "fk_deceased_profile_id_funeral");

            entity.HasIndex(e => e.KinProfileId, "fk_kin_profile_id_funeral");

            entity.HasIndex(e => e.RequesterId, "fk_requester_id_funeral");

            entity.Property(e => e.FuneralId).HasColumnName("funeral_id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.AssignedToId).HasColumnName("assigned_to_id");
            entity.Property(e => e.CauseOfDeath)
                .HasMaxLength(100)
                .HasColumnName("cause_of_death");
            entity.Property(e => e.DateOfDeath)
                .HasColumnType("date")
                .HasColumnName("date_of_death");
            entity.Property(e => e.DeceasedProfileId).HasColumnName("deceased_profile_id");
            entity.Property(e => e.EndTime)
                .HasColumnType("time")
                .HasColumnName("end_time");
            entity.Property(e => e.ImplementationDate)
                .HasColumnType("date")
                .HasColumnName("implementation_date");
            entity.Property(e => e.KinProfileId).HasColumnName("kin_profile_id");
            entity.Property(e => e.KinRelationshipToDeceased)
                .HasMaxLength(50)
                .HasColumnName("kin_relationship_to_deceased");
            entity.Property(e => e.PsaDeathCertNo)
                .HasMaxLength(25)
                .HasColumnName("psa_death_cert_no");
            entity.Property(e => e.RequesterId).HasColumnName("requester_id");
            entity.Property(e => e.StartTime)
                .HasColumnType("time")
                .HasColumnName("start_time");
            entity.Property(e => e.Status)
                .HasColumnType("enum('in progress','scheduled','rescheduled','cancelled','complete')")
                .HasColumnName("status");
            entity.Property(e => e.Venue)
                .HasMaxLength(100)
                .HasColumnName("venue");

            entity.HasOne(d => d.Appointment).WithOne(p => p.Funeral)
                .HasForeignKey<Funeral>(d => d.AppointmentId)
                .HasConstraintName("fk_appointment_id_funeral");

            entity.HasOne(d => d.AssignedTo).WithMany(p => p.FuneralAssignedTos)
                .HasForeignKey(d => d.AssignedToId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_assigned_to_id_funeral");

            entity.HasOne(d => d.DeceasedProfile).WithMany(p => p.FuneralDeceasedProfiles)
                .HasForeignKey(d => d.DeceasedProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_deceased_profile_id_funeral");

            entity.HasOne(d => d.KinProfile).WithMany(p => p.FuneralKinProfiles)
                .HasForeignKey(d => d.KinProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_kin_profile_id_funeral");

            entity.HasOne(d => d.Requester).WithMany(p => p.FuneralRequesters)
                .HasForeignKey(d => d.RequesterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requester_id_funeral");
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
            entity.Property(e => e.AssignedDate)
                .HasColumnType("date")
                .HasColumnName("assigned_date");
            entity.Property(e => e.ChurchId).HasColumnName("church_id");
            entity.Property(e => e.DistrictId).HasColumnName("district_id");
            entity.Property(e => e.EndDate)
                .HasColumnType("date")
                .HasColumnName("end_date");
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

        modelBuilder.Entity<ParentProfile>(entity =>
        {
            entity.HasKey(e => e.ParentProfileId).HasName("PRIMARY");

            entity.ToTable("parent_profile");

            entity.HasIndex(e => e.ChildDedicationId, "fk_child_dedication_id_profile");

            entity.HasIndex(e => e.ProfileId, "fk_profile_id_child_dedication_profile");

            entity.Property(e => e.ParentProfileId).HasColumnName("parent_profile_id");
            entity.Property(e => e.ChildDedicationId).HasColumnName("child_dedication_id");
            entity.Property(e => e.Citizenship)
                .HasMaxLength(50)
                .HasColumnName("citizenship");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.Religion)
                .HasMaxLength(50)
                .HasColumnName("religion");
            entity.Property(e => e.TotalNumOfChildren).HasColumnName("total_num_of_children");

            entity.HasOne(d => d.ChildDedication).WithMany(p => p.ParentProfiles)
                .HasForeignKey(d => d.ChildDedicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_child_dedication_id_profile");

            entity.HasOne(d => d.Profile).WithMany(p => p.ParentProfiles)
                .HasForeignKey(d => d.ProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_profile_id_child_dedication_profile");
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
            entity.Property(e => e.BirthDate)
                .HasColumnType("date")
                .HasColumnName("birth_date");
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
            entity.Property(e => e.IsMember)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_member");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(100)
                .HasColumnName("middle_name");
            entity.Property(e => e.Municipality)
                .HasMaxLength(50)
                .HasColumnName("municipality");
            entity.Property(e => e.ProfileStatus)
                .HasDefaultValueSql("'Active'")
                .HasColumnType("enum('Active','Inactive')")
                .HasColumnName("profile_status");
            entity.Property(e => e.Province)
                .HasMaxLength(50)
                .HasColumnName("province");
            entity.Property(e => e.Sex)
                .HasColumnType("enum('Male','Female')")
                .HasColumnName("sex");
            entity.Property(e => e.Street)
                .HasMaxLength(50)
                .HasColumnName("street");

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
            entity.Property(e => e.JoinedAt)
                .HasColumnType("date")
                .HasColumnName("joined_at");
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

        modelBuilder.Entity<TimeException>(entity =>
        {
            entity.HasKey(e => e.TimeExceptionId).HasName("PRIMARY");

            entity.ToTable("time_exception");

            entity.HasIndex(e => e.AccountId, "fk_account_id_time_exception");

            entity.HasIndex(e => new { e.StartTime, e.EndTime, e.Date, e.AccountId }, "unique_schedule_time_exception").IsUnique();

            entity.Property(e => e.TimeExceptionId).HasColumnName("time_exception_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");
            entity.Property(e => e.EndTime)
                .HasColumnType("time")
                .HasColumnName("end_time");
            entity.Property(e => e.StartTime)
                .HasColumnType("time")
                .HasColumnName("start_time");

            entity.HasOne(d => d.Account).WithMany(p => p.TimeExceptions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_account_id_time_exception");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PRIMARY");

            entity.ToTable("transaction");

            entity.HasIndex(e => e.RecordedById, "IX_transaction_recorded_by_id");

            entity.HasIndex(e => e.TransactionCode, "transaction_code").IsUnique();

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.AccountName)
                .HasMaxLength(100)
                .HasColumnName("account_name");
            entity.Property(e => e.Amount)
                .HasPrecision(15)
                .HasColumnName("amount");
            entity.Property(e => e.BudgetLabel)
                .HasMaxLength(150)
                .HasColumnName("budget_label");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.RecordedById).HasColumnName("recorded_by_id");
            entity.Property(e => e.TransactionCode)
                .HasMaxLength(20)
                .HasColumnName("transaction_code");
            entity.Property(e => e.TransactionDate)
                .HasColumnType("date")
                .HasColumnName("transaction_date");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");

            entity.HasOne(d => d.RecordedBy).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.RecordedById)
                .HasConstraintName("fk_transaction_recorded_by");
        });

        modelBuilder.Entity<Wedding>(entity =>
        {
            entity.HasKey(e => e.WeddingId).HasName("PRIMARY");

            entity.ToTable("wedding");

            entity.HasIndex(e => e.AppointmentId, "appointment_id").IsUnique();

            entity.HasIndex(e => e.CounselId, "counsel_id").IsUnique();

            entity.HasIndex(e => e.AssignedToId, "fk_assigned_to_id_wedding");

            entity.HasIndex(e => e.RequesterId, "fk_requester_id_wedding");

            entity.Property(e => e.WeddingId).HasColumnName("wedding_id");
            entity.Property(e => e.Affidavit).HasColumnName("affidavit");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.AssignedToId).HasColumnName("assigned_to_id");
            entity.Property(e => e.CounselId).HasColumnName("counsel_id");
            entity.Property(e => e.EndTime)
                .HasColumnType("time")
                .HasColumnName("end_time");
            entity.Property(e => e.ImplementationDate)
                .HasColumnType("date")
                .HasColumnName("implementation_date");
            entity.Property(e => e.MarriageCert).HasColumnName("marriage_cert");
            entity.Property(e => e.MarriageType)
                .HasColumnType("enum('mixed','standard','intrafaith')")
                .HasColumnName("marriage_type");
            entity.Property(e => e.RecommendationLetter).HasColumnName("recommendation_letter");
            entity.Property(e => e.RequesterId).HasColumnName("requester_id");
            entity.Property(e => e.StartTime)
                .HasColumnType("time")
                .HasColumnName("start_time");
            entity.Property(e => e.Status)
                .HasColumnType("enum('in progress','scheduled','rescheduled','cancelled','complete')")
                .HasColumnName("status");
            entity.Property(e => e.Venue)
                .HasMaxLength(100)
                .HasColumnName("venue");

            entity.HasOne(d => d.Appointment).WithOne(p => p.Wedding)
                .HasForeignKey<Wedding>(d => d.AppointmentId)
                .HasConstraintName("fk_appointment_id_wedding");

            entity.HasOne(d => d.AssignedTo).WithMany(p => p.WeddingAssignedTos)
                .HasForeignKey(d => d.AssignedToId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_assigned_to_id_wedding");

            entity.HasOne(d => d.Counsel).WithOne(p => p.Wedding)
                .HasForeignKey<Wedding>(d => d.CounselId)
                .HasConstraintName("fk_counsel_id_wedding");

            entity.HasOne(d => d.Requester).WithMany(p => p.WeddingRequesters)
                .HasForeignKey(d => d.RequesterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requester_id_wedding");
        });

        modelBuilder.Entity<WeddingProfile>(entity =>
        {
            entity.HasKey(e => e.WeddingProfileId).HasName("PRIMARY");

            entity.ToTable("wedding_profile");

            entity.HasIndex(e => e.ProfileId, "fk_profile_id_wedding_profile");

            entity.HasIndex(e => e.WeddingId, "fk_wedding_id_profile");

            entity.Property(e => e.WeddingProfileId).HasColumnName("wedding_profile_id");
            entity.Property(e => e.BirthCert).HasColumnName("birth_cert");
            entity.Property(e => e.Cenomar)
                .HasDefaultValueSql("'0'")
                .HasColumnName("cenomar");
            entity.Property(e => e.Occupation)
                .HasMaxLength(50)
                .HasColumnName("occupation");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.Religion)
                .HasMaxLength(50)
                .HasColumnName("religion");
            entity.Property(e => e.WeddingId).HasColumnName("wedding_id");

            entity.HasOne(d => d.Profile).WithMany(p => p.WeddingProfiles)
                .HasForeignKey(d => d.ProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_profile_id_wedding_profile");

            entity.HasOne(d => d.Wedding).WithMany(p => p.WeddingProfiles)
                .HasForeignKey(d => d.WeddingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_wedding_id_profile");
        });

        modelBuilder.Entity<WeddingWitness>(entity =>
        {
            entity.HasKey(e => e.WeddingWitnessId).HasName("PRIMARY");

            entity.ToTable("wedding_witness");

            entity.HasIndex(e => e.ProfileId, "fk_profile_id_wedding_witness");

            entity.HasIndex(e => e.WeddingId, "fk_wedding_id_witness");

            entity.Property(e => e.WeddingWitnessId).HasColumnName("wedding_witness_id");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.WeddingId).HasColumnName("wedding_id");

            entity.HasOne(d => d.Profile).WithMany(p => p.WeddingWitnesses)
                .HasForeignKey(d => d.ProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_profile_id_wedding_witness");

            entity.HasOne(d => d.Wedding).WithMany(p => p.WeddingWitnesses)
                .HasForeignKey(d => d.WeddingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_wedding_id_witness");
        });

        modelBuilder.Entity<WeeklyConfig>(entity =>
        {
            entity.HasKey(e => e.WeeklyConfigId).HasName("PRIMARY");

            entity.ToTable("weekly_config");

            entity.HasIndex(e => e.AccountId, "fk_account_id_weekly_config");

            entity.HasIndex(e => new { e.StartTime, e.EndTime, e.Day, e.AccountId }, "unique_schedule_weekly_config").IsUnique();

            entity.Property(e => e.WeeklyConfigId).HasColumnName("weekly_config_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Day).HasColumnName("day");
            entity.Property(e => e.EndTime)
                .HasColumnType("time")
                .HasColumnName("end_time");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.StartTime)
                .HasColumnType("time")
                .HasColumnName("start_time");

            entity.HasOne(d => d.Account).WithMany(p => p.WeeklyConfigs)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_account_id_weekly_config");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
