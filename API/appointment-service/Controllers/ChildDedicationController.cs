using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using appointment_service.Models;
using appointment_service.DTOs;

namespace appointment_service.Controllers;

[ApiController]
[Route("[controller]")]
public class ChildDedicationController : ControllerBase
{
    private readonly CmisContext _context;
    public ChildDedicationController(CmisContext context) { _context = context; }

    [HttpGet("all")]
    public async Task<ActionResult> GetAllChildDedications()
    {
        var list = await _context.ChildDedications
            .Include(cd => cd.ChildProfile)
            .Select(cd => new
            {
                cd.ChildDedicationId,
                cd.ChildProfileId,
                ChildName = cd.ChildProfile != null ? $"{cd.ChildProfile.FirstName} {cd.ChildProfile.LastName}" : "Unknown",
                cd.Venue,
                cd.ChildPlaceOfBirth,
                cd.ChildWeightAtBirth,
                cd.BirthCert,
                ParentsMarriageDate = cd.ParentsMarriageDate.ToString("yyyy-MM-dd"),
                cd.ParentsMarriagePlaceMunicipality,
                cd.ParentsMarriagePlaceProvince,
                cd.ParentsMarriagePlaceCountry,
                ImplementationDate = cd.ImplementationDate.HasValue ? cd.ImplementationDate.Value.ToString("yyyy-MM-dd") : null,
                StartTime = cd.StartTime.HasValue ? cd.StartTime.Value.ToString(@"hh\:mm\:ss") : null,
                EndTime = cd.EndTime.HasValue ? cd.EndTime.Value.ToString(@"hh\:mm\:ss") : null,
                cd.Status,
                cd.RequesterId,
                cd.AssignedToId,
                cd.AppointmentId
            })
            .ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetChildDedication(int id)
    {
        var cd = await _context.ChildDedications
            .Include(x => x.ChildProfile)
            .Include(x => x.ParentProfiles).ThenInclude(pp => pp.Profile)
            .Include(x => x.ChildDedicationWitnesses).ThenInclude(cw => cw.Profile)
            .FirstOrDefaultAsync(x => x.ChildDedicationId == id);
        if (cd == null) return NotFound();
        return Ok(new {
            cd.ChildDedicationId, cd.ChildProfileId,
            ChildFirstName = cd.ChildProfile?.FirstName, ChildMiddleName = cd.ChildProfile?.MiddleName, ChildLastName = cd.ChildProfile?.LastName,
            ChildSex = cd.ChildProfile?.Sex, ChildContact = cd.ChildProfile?.ContactNumber, ChildBirthDate = cd.ChildProfile?.BirthDate?.ToString("yyyy-MM-dd"),
            ChildStatus = cd.ChildProfile?.ProfileStatus,
            cd.Venue, cd.ChildPlaceOfBirth, cd.ChildWeightAtBirth, cd.BirthCert,
            ParentsMarriageDate = cd.ParentsMarriageDate.ToString("yyyy-MM-dd"),
            cd.ParentsMarriagePlaceMunicipality, cd.ParentsMarriagePlaceProvince, cd.ParentsMarriagePlaceCountry,
            ImplementationDate = cd.ImplementationDate.HasValue ? cd.ImplementationDate.Value.ToString("yyyy-MM-dd") : null,
            StartTime = cd.StartTime.HasValue ? cd.StartTime.Value.ToString(@"hh\:mm\:ss") : null,
            EndTime = cd.EndTime.HasValue ? cd.EndTime.Value.ToString(@"hh\:mm\:ss") : null,
            cd.Status, cd.RequesterId, cd.AssignedToId, cd.AppointmentId,
            ParentProfiles = cd.ParentProfiles.Select(pp => new {
                pp.ParentProfileId, pp.ProfileId, pp.Citizenship, pp.Religion, pp.TotalNumOfChildren,
                ProfileFirstName = pp.Profile?.FirstName, ProfileMiddleName = pp.Profile?.MiddleName, ProfileLastName = pp.Profile?.LastName,
                ProfileSex = pp.Profile?.Sex, ProfileContact = pp.Profile?.ContactNumber, ProfileBirthDate = pp.Profile?.BirthDate?.ToString("yyyy-MM-dd"),
                ProfileStatus = pp.Profile?.ProfileStatus
            }).ToList(),
            Witnesses = cd.ChildDedicationWitnesses.Select(cw => new {
                cw.ChildDedicationWitnessId, cw.ProfileId,
                ProfileFirstName = cw.Profile?.FirstName, ProfileMiddleName = cw.Profile?.MiddleName, ProfileLastName = cw.Profile?.LastName,
                ProfileSex = cw.Profile?.Sex, ProfileContact = cw.Profile?.ContactNumber, ProfileBirthDate = cw.Profile?.BirthDate?.ToString("yyyy-MM-dd"),
                ProfileStatus = cw.Profile?.ProfileStatus
            }).ToList()
        });
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateChildDedication([FromBody] ChildDedicationCreateDto dto)
    {
        var childDedication = new ChildDedication
        {
            RequesterId = dto.RequesterId,
            AssignedToId = dto.AssignedToId,
            ChildProfileId = dto.ChildProfileId,
            AppointmentId = dto.AppointmentId,
            ImplementationDate = dto.ImplementationDate != null ? DateTime.Parse(dto.ImplementationDate) : null,
            StartTime = dto.StartTime != null ? TimeSpan.Parse(dto.StartTime) : null,
            EndTime = dto.EndTime != null ? TimeSpan.Parse(dto.EndTime) : null,
            Venue = dto.Venue,
            CounselId = dto.CounselId,
            BirthCert = dto.BirthCert,
            ChildPlaceOfBirth = dto.ChildPlaceOfBirth,
            ChildWeightAtBirth = dto.ChildWeightAtBirth,
            ParentsMarriageDate = DateTime.Parse(dto.ParentsMarriageDate),
            ParentsMarriagePlaceMunicipality = dto.ParentsMarriagePlaceMunicipality,
            ParentsMarriagePlaceProvince = dto.ParentsMarriagePlaceProvince,
            ParentsMarriagePlaceCountry = dto.ParentsMarriagePlaceCountry,
            Status = dto.Status
        };

        if (dto.ParentProfiles != null && dto.ParentProfiles.Any())
        {
            childDedication.ParentProfiles = dto.ParentProfiles.Select(pp => new ParentProfile
            {
                ProfileId = pp.ProfileId,
                Citizenship = pp.Citizenship,
                Religion = pp.Religion,
                TotalNumOfChildren = pp.TotalNumOfChildren
            }).ToList();
        }

        if (dto.WitnessProfileIds != null && dto.WitnessProfileIds.Any())
        {
            childDedication.ChildDedicationWitnesses = dto.WitnessProfileIds.Select(id => new ChildDedicationWitness
            {
                ProfileId = id
            }).ToList();
        }

        _context.ChildDedications.Add(childDedication);
        await _context.SaveChangesAsync();
        return CreatedAtAction(null, new { id = childDedication.ChildDedicationId }, childDedication);
    }

    [HttpPut("update/{id}")]
    public async Task<ActionResult> UpdateChildDedication(int id, [FromBody] ChildDedicationCreateDto dto)
    {
        var childDedication = await _context.ChildDedications
            .Include(cd => cd.ParentProfiles)
            .Include(cd => cd.ChildDedicationWitnesses)
            .FirstOrDefaultAsync(cd => cd.ChildDedicationId == id);
        if (childDedication == null) return NotFound();

        // Update scalar fields
        childDedication.RequesterId = dto.RequesterId;
        childDedication.AssignedToId = dto.AssignedToId;
        childDedication.ChildProfileId = dto.ChildProfileId;
        childDedication.AppointmentId = dto.AppointmentId;
        childDedication.ImplementationDate = dto.ImplementationDate != null ? DateTime.Parse(dto.ImplementationDate) : null;
        childDedication.StartTime = dto.StartTime != null ? TimeSpan.Parse(dto.StartTime) : null;
        childDedication.EndTime = dto.EndTime != null ? TimeSpan.Parse(dto.EndTime) : null;
        childDedication.Venue = dto.Venue;
        childDedication.CounselId = dto.CounselId;
        childDedication.BirthCert = dto.BirthCert;
        childDedication.ChildPlaceOfBirth = dto.ChildPlaceOfBirth;
        childDedication.ChildWeightAtBirth = dto.ChildWeightAtBirth;
        childDedication.ParentsMarriageDate = DateTime.Parse(dto.ParentsMarriageDate);
        childDedication.ParentsMarriagePlaceMunicipality = dto.ParentsMarriagePlaceMunicipality;
        childDedication.ParentsMarriagePlaceProvince = dto.ParentsMarriagePlaceProvince;
        childDedication.ParentsMarriagePlaceCountry = dto.ParentsMarriagePlaceCountry;
        childDedication.Status = dto.Status;

        // Clear and re-insert parent profiles
        _context.Set<ParentProfile>().RemoveRange(childDedication.ParentProfiles);
        if (dto.ParentProfiles != null && dto.ParentProfiles.Any())
        {
            foreach (var pp in dto.ParentProfiles)
            {
                childDedication.ParentProfiles.Add(new ParentProfile { ProfileId = pp.ProfileId, Citizenship = pp.Citizenship, Religion = pp.Religion, TotalNumOfChildren = pp.TotalNumOfChildren });
            }
        }

        // Clear and re-insert witnesses
        _context.Set<ChildDedicationWitness>().RemoveRange(childDedication.ChildDedicationWitnesses);
        if (dto.WitnessProfileIds != null && dto.WitnessProfileIds.Any())
        {
            foreach (var wid in dto.WitnessProfileIds)
            {
                childDedication.ChildDedicationWitnesses.Add(new ChildDedicationWitness { ProfileId = wid });
            }
        }

        await _context.SaveChangesAsync();
        return Ok(childDedication);
    }
}

