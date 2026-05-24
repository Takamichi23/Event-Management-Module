using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public int ProfileId { get; set; }

    public int RoleId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Appointment> AppointmentAssignedTos { get; set; } = new List<Appointment>();

    public virtual ICollection<Appointment> AppointmentRequesters { get; set; } = new List<Appointment>();

    public virtual ICollection<Baptism> BaptismAssignedTos { get; set; } = new List<Baptism>();

    public virtual ICollection<Baptism> BaptismRequesters { get; set; } = new List<Baptism>();

    public virtual ICollection<ChildDedication> ChildDedicationAssignedTos { get; set; } = new List<ChildDedication>();

    public virtual ICollection<ChildDedication> ChildDedicationRequesters { get; set; } = new List<ChildDedication>();

    public virtual ICollection<Counsel> CounselAssignedTos { get; set; } = new List<Counsel>();

    public virtual ICollection<Counsel> CounselRequesters { get; set; } = new List<Counsel>();

    public virtual ICollection<DayException> DayExceptions { get; set; } = new List<DayException>();

    public virtual ICollection<Funeral> FuneralAssignedTos { get; set; } = new List<Funeral>();

    public virtual ICollection<Funeral> FuneralRequesters { get; set; } = new List<Funeral>();

    public virtual Profile Profile { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<TimeException> TimeExceptions { get; set; } = new List<TimeException>();

    public virtual ICollection<Wedding> WeddingAssignedTos { get; set; } = new List<Wedding>();

    public virtual ICollection<Wedding> WeddingRequesters { get; set; } = new List<Wedding>();

    public virtual ICollection<WeeklyConfig> WeeklyConfigs { get; set; } = new List<WeeklyConfig>();
}
