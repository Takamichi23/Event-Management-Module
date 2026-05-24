using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using appointment_service.Models;
using appointment_service.DTOs;

namespace appointment_service.Controllers;

[ApiController]
[Route("[controller]")]
public class WeddingController : ControllerBase
{
    private readonly CmisContext _context;
    public WeddingController(CmisContext context) { _context = context; }

    [HttpGet("all")]
    public async Task<ActionResult> GetAllWeddings()
    {
        var list = await _context.Weddings
            .Select(w => new
            {
                w.WeddingId,
                w.Venue,
                w.MarriageType,
                ImplementationDate = w.ImplementationDate.HasValue ? w.ImplementationDate.Value.ToString("yyyy-MM-dd") : null,
                StartTime = w.StartTime.HasValue ? w.StartTime.Value.ToString(@"hh\:mm\:ss") : null,
                EndTime = w.EndTime.HasValue ? w.EndTime.Value.ToString(@"hh\:mm\:ss") : null,
                w.MarriageCert,
                w.RecommendationLetter,
                w.Affidavit,
                w.CounselId,
                w.Status,
                w.RequesterId,
                w.AssignedToId,
                w.AppointmentId
            })
            .ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetWedding(int id)
    {
        var w = await _context.Weddings
            .Include(x => x.WeddingProfiles).ThenInclude(wp => wp.Profile)
            .Include(x => x.WeddingWitnesses).ThenInclude(ww => ww.Profile)
            .FirstOrDefaultAsync(x => x.WeddingId == id);
        if (w == null) return NotFound();
        return Ok(new {
            w.WeddingId, w.Venue, w.MarriageType,
            ImplementationDate = w.ImplementationDate.HasValue ? w.ImplementationDate.Value.ToString("yyyy-MM-dd") : null,
            StartTime = w.StartTime.HasValue ? w.StartTime.Value.ToString(@"hh\:mm\:ss") : null,
            EndTime = w.EndTime.HasValue ? w.EndTime.Value.ToString(@"hh\:mm\:ss") : null,
            w.MarriageCert, w.RecommendationLetter, w.Affidavit, w.CounselId, w.Status, w.RequesterId, w.AssignedToId, w.AppointmentId,
            WeddingProfiles = w.WeddingProfiles.Select(wp => new {
                wp.WeddingProfileId, wp.ProfileId, wp.BirthCert, wp.Occupation, wp.Religion, wp.Cenomar,
                ProfileFirstName = wp.Profile?.FirstName, ProfileMiddleName = wp.Profile?.MiddleName, ProfileLastName = wp.Profile?.LastName,
                ProfileSex = wp.Profile?.Sex, ProfileContact = wp.Profile?.ContactNumber, ProfileBirthDate = wp.Profile?.BirthDate?.ToString("yyyy-MM-dd"),
                ProfileStatus = wp.Profile?.ProfileStatus
            }).ToList(),
            WeddingWitnesses = w.WeddingWitnesses.Select(ww => new {
                ww.WeddingWitnessId, ww.ProfileId,
                ProfileFirstName = ww.Profile?.FirstName, ProfileMiddleName = ww.Profile?.MiddleName, ProfileLastName = ww.Profile?.LastName,
                ProfileSex = ww.Profile?.Sex, ProfileContact = ww.Profile?.ContactNumber, ProfileBirthDate = ww.Profile?.BirthDate?.ToString("yyyy-MM-dd"),
                ProfileStatus = ww.Profile?.ProfileStatus
            }).ToList()
        });
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateWedding([FromBody] WeddingCreateDto dto)
    {
        var wedding = new Wedding
        {
            RequesterId = dto.RequesterId,
            AssignedToId = dto.AssignedToId,
            AppointmentId = dto.AppointmentId,
            ImplementationDate = dto.ImplementationDate != null ? DateTime.Parse(dto.ImplementationDate) : null,
            Venue = dto.Venue,
            StartTime = dto.StartTime != null ? TimeSpan.Parse(dto.StartTime) : null,
            EndTime = dto.EndTime != null ? TimeSpan.Parse(dto.EndTime) : null,
            CounselId = dto.CounselId,
            MarriageCert = dto.MarriageCert,
            RecommendationLetter = dto.RecommendationLetter,
            Affidavit = dto.Affidavit,
            MarriageType = dto.MarriageType,
            Status = dto.Status
        };
        
        if (dto.WeddingProfiles != null && dto.WeddingProfiles.Any())
        {
            wedding.WeddingProfiles = dto.WeddingProfiles.Select(wp => new WeddingProfile
            {
                ProfileId = wp.ProfileId,
                BirthCert = wp.BirthCert,
                Occupation = wp.Occupation,
                Religion = wp.Religion,
                Cenomar = wp.Cenomar
            }).ToList();
        }

        if (dto.WeddingWitnessProfileIds != null && dto.WeddingWitnessProfileIds.Any())
        {
            wedding.WeddingWitnesses = dto.WeddingWitnessProfileIds.Select(id => new WeddingWitness
            {
                ProfileId = id
            }).ToList();
        }

        _context.Weddings.Add(wedding);
        await _context.SaveChangesAsync();
        return CreatedAtAction(null, new { id = wedding.WeddingId }, wedding);
    }

    [HttpPut("update/{id}")]
    public async Task<ActionResult> UpdateWedding(int id, [FromBody] WeddingCreateDto dto)
    {
        var wedding = await _context.Weddings
            .Include(w => w.WeddingProfiles)
            .Include(w => w.WeddingWitnesses)
            .FirstOrDefaultAsync(w => w.WeddingId == id);
        if (wedding == null) return NotFound();

        // Update scalar fields
        wedding.RequesterId = dto.RequesterId;
        wedding.AssignedToId = dto.AssignedToId;
        wedding.AppointmentId = dto.AppointmentId;
        wedding.ImplementationDate = dto.ImplementationDate != null ? DateTime.Parse(dto.ImplementationDate) : null;
        wedding.Venue = dto.Venue;
        wedding.StartTime = dto.StartTime != null ? TimeSpan.Parse(dto.StartTime) : null;
        wedding.EndTime = dto.EndTime != null ? TimeSpan.Parse(dto.EndTime) : null;
        wedding.CounselId = dto.CounselId;
        wedding.MarriageCert = dto.MarriageCert;
        wedding.RecommendationLetter = dto.RecommendationLetter;
        wedding.Affidavit = dto.Affidavit;
        wedding.MarriageType = dto.MarriageType;
        wedding.Status = dto.Status;

        // Clear and re-insert profiles
        _context.Set<WeddingProfile>().RemoveRange(wedding.WeddingProfiles);
        if (dto.WeddingProfiles != null && dto.WeddingProfiles.Any())
        {
            foreach (var wp in dto.WeddingProfiles)
            {
                wedding.WeddingProfiles.Add(new WeddingProfile { ProfileId = wp.ProfileId, BirthCert = wp.BirthCert, Occupation = wp.Occupation, Religion = wp.Religion, Cenomar = wp.Cenomar });
            }
        }

        // Clear and re-insert witnesses
        _context.Set<WeddingWitness>().RemoveRange(wedding.WeddingWitnesses);
        if (dto.WeddingWitnessProfileIds != null && dto.WeddingWitnessProfileIds.Any())
        {
            foreach (var wid in dto.WeddingWitnessProfileIds)
            {
                wedding.WeddingWitnesses.Add(new WeddingWitness { ProfileId = wid });
            }
        }

        await _context.SaveChangesAsync();
        return Ok(wedding);
    }
}

