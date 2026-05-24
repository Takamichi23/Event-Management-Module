using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using appointment_service.Models;
using appointment_service.DTOs;

namespace appointment_service.Controllers;

[ApiController]
[Route("[controller]")]
public class CounselController : ControllerBase
{
    private readonly CmisContext _context;
    public CounselController(CmisContext context) { _context = context; }

    [HttpGet("all")]
    public async Task<ActionResult> GetAllCounsels()
    {
        var list = await _context.Counsels
            .Include(c => c.Profile)
            .Select(c => new
            {
                c.CounselId,
                c.ProfileId,
                PersonName = c.Profile != null ? $"{c.Profile.FirstName} {c.Profile.LastName}" : "Unknown",
                c.CounselFor,
                ImplementationDate = c.ImplementationDate.HasValue ? c.ImplementationDate.Value.ToString("yyyy-MM-dd") : null,
                StartTime = c.StartTime.HasValue ? c.StartTime.Value.ToString(@"hh\:mm\:ss") : null,
                EndTime = c.EndTime.HasValue ? c.EndTime.Value.ToString(@"hh\:mm\:ss") : null,
                c.Status,
                c.RequesterId,
                c.AssignedToId,
                c.AppointmentId
            })
            .ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetCounsel(int id)
    {
        var c = await _context.Counsels.Include(x => x.Profile).FirstOrDefaultAsync(x => x.CounselId == id);
        if (c == null) return NotFound();
        return Ok(new {
            c.CounselId, c.ProfileId,
            PersonName = c.Profile != null ? $"{c.Profile.FirstName} {c.Profile.LastName}" : "Unknown",
            ProfileFirstName = c.Profile?.FirstName, ProfileMiddleName = c.Profile?.MiddleName, ProfileLastName = c.Profile?.LastName,
            ProfileSex = c.Profile?.Sex, ProfileContact = c.Profile?.ContactNumber, ProfileBirthDate = c.Profile?.BirthDate?.ToString("yyyy-MM-dd"),
            ProfileStatus = c.Profile?.ProfileStatus,
            c.CounselFor,
            ImplementationDate = c.ImplementationDate.HasValue ? c.ImplementationDate.Value.ToString("yyyy-MM-dd") : null,
            StartTime = c.StartTime.HasValue ? c.StartTime.Value.ToString(@"hh\:mm\:ss") : null,
            EndTime = c.EndTime.HasValue ? c.EndTime.Value.ToString(@"hh\:mm\:ss") : null,
            c.Status, c.RequesterId, c.AssignedToId, c.AppointmentId
        });
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateCounsel([FromBody] CounselCreateDto dto)
    {
        var counsel = new Counsel
        {
            RequesterId = dto.RequesterId,
            AssignedToId = dto.AssignedToId,
            ProfileId = dto.ProfileId,
            AppointmentId = dto.AppointmentId,
            ImplementationDate = dto.ImplementationDate != null ? DateTime.Parse(dto.ImplementationDate) : null,
            StartTime = dto.StartTime != null ? TimeSpan.Parse(dto.StartTime) : null,
            EndTime = dto.EndTime != null ? TimeSpan.Parse(dto.EndTime) : null,
            CounselFor = dto.CounselFor,
            Status = dto.Status
        };
        _context.Counsels.Add(counsel);
        await _context.SaveChangesAsync();
        return CreatedAtAction(null, new { id = counsel.CounselId }, counsel);
    }

    [HttpPut("update/{id}")]
    public async Task<ActionResult> UpdateCounsel(int id, [FromBody] CounselCreateDto dto)
    {
        var counsel = await _context.Counsels.FindAsync(id);
        if (counsel == null) return NotFound();
        counsel.RequesterId = dto.RequesterId;
        counsel.AssignedToId = dto.AssignedToId;
        counsel.ProfileId = dto.ProfileId;
        counsel.AppointmentId = dto.AppointmentId;
        counsel.ImplementationDate = dto.ImplementationDate != null ? DateTime.Parse(dto.ImplementationDate) : null;
        counsel.StartTime = dto.StartTime != null ? TimeSpan.Parse(dto.StartTime) : null;
        counsel.EndTime = dto.EndTime != null ? TimeSpan.Parse(dto.EndTime) : null;
        counsel.CounselFor = dto.CounselFor;
        counsel.Status = dto.Status;
        await _context.SaveChangesAsync();
        return Ok(counsel);
    }
}
