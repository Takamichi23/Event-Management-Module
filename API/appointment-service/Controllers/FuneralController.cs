using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using appointment_service.Models;
using appointment_service.DTOs;

namespace appointment_service.Controllers;

[ApiController]
[Route("[controller]")]
public class FuneralController : ControllerBase
{
    private readonly CmisContext _context;
    public FuneralController(CmisContext context) { _context = context; }

    [HttpGet("all")]
    public async Task<ActionResult> GetAllFunerals()
    {
        var list = await _context.Funerals
            .Include(f => f.DeceasedProfile)
            .Include(f => f.KinProfile)
            .Select(f => new
            {
                f.FuneralId,
                f.DeceasedProfileId,
                DeceasedName = f.DeceasedProfile != null ? $"{f.DeceasedProfile.FirstName} {f.DeceasedProfile.LastName}" : "Unknown",
                f.KinProfileId,
                KinName = f.KinProfile != null ? $"{f.KinProfile.FirstName} {f.KinProfile.LastName}" : "Unknown",
                f.KinRelationshipToDeceased,
                DateOfDeath = f.DateOfDeath.ToString("yyyy-MM-dd"),
                f.CauseOfDeath,
                f.PsaDeathCertNo,
                f.Venue,
                ImplementationDate = f.ImplementationDate.HasValue ? f.ImplementationDate.Value.ToString("yyyy-MM-dd") : null,
                StartTime = f.StartTime.HasValue ? f.StartTime.Value.ToString(@"hh\:mm\:ss") : null,
                EndTime = f.EndTime.HasValue ? f.EndTime.Value.ToString(@"hh\:mm\:ss") : null,
                f.Status,
                f.RequesterId,
                f.AssignedToId,
                f.AppointmentId
            })
            .ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetFuneral(int id)
    {
        var f = await _context.Funerals.Include(x => x.DeceasedProfile).Include(x => x.KinProfile).FirstOrDefaultAsync(x => x.FuneralId == id);
        if (f == null) return NotFound();
        return Ok(new {
            f.FuneralId, f.DeceasedProfileId, f.KinProfileId,
            DeceasedFirstName = f.DeceasedProfile?.FirstName, DeceasedMiddleName = f.DeceasedProfile?.MiddleName, DeceasedLastName = f.DeceasedProfile?.LastName,
            DeceasedSex = f.DeceasedProfile?.Sex, DeceasedContact = f.DeceasedProfile?.ContactNumber, DeceasedBirthDate = f.DeceasedProfile?.BirthDate?.ToString("yyyy-MM-dd"),
            DeceasedStatus = f.DeceasedProfile?.ProfileStatus,
            KinFirstName = f.KinProfile?.FirstName, KinMiddleName = f.KinProfile?.MiddleName, KinLastName = f.KinProfile?.LastName,
            KinSex = f.KinProfile?.Sex, KinContact = f.KinProfile?.ContactNumber, KinBirthDate = f.KinProfile?.BirthDate?.ToString("yyyy-MM-dd"),
            KinStatus = f.KinProfile?.ProfileStatus,
            f.KinRelationshipToDeceased,
            DateOfDeath = f.DateOfDeath.ToString("yyyy-MM-dd"),
            f.CauseOfDeath, f.PsaDeathCertNo, f.Venue,
            ImplementationDate = f.ImplementationDate.HasValue ? f.ImplementationDate.Value.ToString("yyyy-MM-dd") : null,
            StartTime = f.StartTime.HasValue ? f.StartTime.Value.ToString(@"hh\:mm\:ss") : null,
            EndTime = f.EndTime.HasValue ? f.EndTime.Value.ToString(@"hh\:mm\:ss") : null,
            f.Status, f.RequesterId, f.AssignedToId, f.AppointmentId
        });
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateFuneral([FromBody] FuneralCreateDto dto)
    {
        var funeral = new Funeral
        {
            RequesterId = dto.RequesterId,
            AssignedToId = dto.AssignedToId,
            DeceasedProfileId = dto.DeceasedProfileId,
            KinProfileId = dto.KinProfileId,
            AppointmentId = dto.AppointmentId,
            StartTime = dto.StartTime != null ? TimeSpan.Parse(dto.StartTime) : null,
            EndTime = dto.EndTime != null ? TimeSpan.Parse(dto.EndTime) : null,
            ImplementationDate = dto.ImplementationDate != null ? DateTime.Parse(dto.ImplementationDate) : null,
            Venue = dto.Venue,
            DateOfDeath = DateTime.Parse(dto.DateOfDeath),
            CauseOfDeath = dto.CauseOfDeath,
            PsaDeathCertNo = dto.PsaDeathCertNo,
            KinRelationshipToDeceased = dto.KinRelationshipToDeceased,
            Status = dto.Status
        };
        _context.Funerals.Add(funeral);
        await _context.SaveChangesAsync();
        return CreatedAtAction(null, new { id = funeral.FuneralId }, funeral);
    }

    [HttpPut("update/{id}")]
    public async Task<ActionResult> UpdateFuneral(int id, [FromBody] FuneralCreateDto dto)
    {
        var funeral = await _context.Funerals.FindAsync(id);
        if (funeral == null) return NotFound();
        funeral.RequesterId = dto.RequesterId;
        funeral.AssignedToId = dto.AssignedToId;
        funeral.DeceasedProfileId = dto.DeceasedProfileId;
        funeral.KinProfileId = dto.KinProfileId;
        funeral.AppointmentId = dto.AppointmentId;
        funeral.StartTime = dto.StartTime != null ? TimeSpan.Parse(dto.StartTime) : null;
        funeral.EndTime = dto.EndTime != null ? TimeSpan.Parse(dto.EndTime) : null;
        funeral.ImplementationDate = dto.ImplementationDate != null ? DateTime.Parse(dto.ImplementationDate) : null;
        funeral.Venue = dto.Venue;
        funeral.DateOfDeath = DateTime.Parse(dto.DateOfDeath);
        funeral.CauseOfDeath = dto.CauseOfDeath;
        funeral.PsaDeathCertNo = dto.PsaDeathCertNo;
        funeral.KinRelationshipToDeceased = dto.KinRelationshipToDeceased;
        funeral.Status = dto.Status;
        await _context.SaveChangesAsync();
        return Ok(funeral);
    }
}
