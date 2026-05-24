namespace appointment_service.DTOs;

public class ChildDedicationCreateDto
{
    public int RequesterId { get; set; }
    public int AssignedToId { get; set; }
    public int ChildProfileId { get; set; }
    public int? AppointmentId { get; set; }
    public string? ImplementationDate { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
    public string? Venue { get; set; }
    public int? CounselId { get; set; }
    public bool? BirthCert { get; set; }
    public string ChildPlaceOfBirth { get; set; } = null!;
    public decimal ChildWeightAtBirth { get; set; }
    public string ParentsMarriageDate { get; set; } = null!;
    public string ParentsMarriagePlaceMunicipality { get; set; } = null!;
    public string ParentsMarriagePlaceProvince { get; set; } = null!;
    public string ParentsMarriagePlaceCountry { get; set; } = null!;
    public string Status { get; set; } = null!;
    public List<ParentProfileDto> ParentProfiles { get; set; } = new();
    public List<int> WitnessProfileIds { get; set; } = new();
}

public class ParentProfileDto
{
    public int ProfileId { get; set; }
    public string Citizenship { get; set; } = "";
    public string Religion { get; set; } = "";
    public int TotalNumOfChildren { get; set; }
}
