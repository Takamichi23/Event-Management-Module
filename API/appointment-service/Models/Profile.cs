using System;
using System.Collections.Generic;

namespace appointment_service.Models;

public partial class Profile
{
    public int ProfileId { get; set; }

    public int ChurchId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public DateTime? BirthDate { get; set; }

    public string? ContactNumber { get; set; }

    public string ProfileStatus { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public string? Street { get; set; }

    public string? Municipality { get; set; }

    public string? Province { get; set; }

    public bool? IsMember { get; set; }

    public string? Address { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Baptism> Baptisms { get; set; } = new List<Baptism>();

    public virtual ICollection<BudgetProposal> BudgetProposalReviewedBies { get; set; } = new List<BudgetProposal>();

    public virtual ICollection<BudgetProposal> BudgetProposalSubmittedBies { get; set; } = new List<BudgetProposal>();

    public virtual ICollection<ChildDedicationWitness> ChildDedicationWitnesses { get; set; } = new List<ChildDedicationWitness>();

    public virtual ICollection<ChildDedication> ChildDedications { get; set; } = new List<ChildDedication>();

    public virtual Church Church { get; set; } = null!;

    public virtual ICollection<Counsel> Counsels { get; set; } = new List<Counsel>();

    public virtual ICollection<Funeral> FuneralDeceasedProfiles { get; set; } = new List<Funeral>();

    public virtual ICollection<Funeral> FuneralKinProfiles { get; set; } = new List<Funeral>();

    public virtual ICollection<LeadershipAssignment> LeadershipAssignments { get; set; } = new List<LeadershipAssignment>();

    public virtual ICollection<ParentProfile> ParentProfiles { get; set; } = new List<ParentProfile>();

    public virtual ICollection<ProfileMinistry> ProfileMinistries { get; set; } = new List<ProfileMinistry>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<WeddingProfile> WeddingProfiles { get; set; } = new List<WeddingProfile>();

    public virtual ICollection<WeddingWitness> WeddingWitnesses { get; set; } = new List<WeddingWitness>();
}
