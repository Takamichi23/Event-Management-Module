using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using appointment_service.Models;
using appointment_service.DTOs;

namespace appointment_service.Controllers;

[ApiController]
[Route("[controller]")]
public class BaptismController : ControllerBase
{
    private readonly CmisContext _context;
    public BaptismController(CmisContext context) { _context = context; }

    [HttpGet("all")]
    public async Task<ActionResult> GetAllBaptisms()
    {
        var list = await _context.Baptisms
            .Include(b => b.Profile)
            .Select(b => new
            {
                b.BaptismId,
                b.ProfileId,
                PersonName = b.Profile != null ? $"{b.Profile.FirstName} {b.Profile.LastName}" : "Unknown",
                b.Occupation,
                b.Venue,
                ImplementationDate = b.ImplementationDate.HasValue ? b.ImplementationDate.Value.ToString("yyyy-MM-dd") : null,
                StartTime = b.StartTime.HasValue ? b.StartTime.Value.ToString(@"hh\:mm\:ss") : null,
                EndTime = b.EndTime.HasValue ? b.EndTime.Value.ToString(@"hh\:mm\:ss") : null,
                b.Status,
                b.RequesterId,
                b.AssignedToId,
                b.AppointmentId
            })
            .ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetBaptism(int id)
    {
        var b = await _context.Baptisms.Include(x => x.Profile).FirstOrDefaultAsync(x => x.BaptismId == id);
        if (b == null) return NotFound();
        return Ok(new {
            b.BaptismId, b.ProfileId,
            PersonName = b.Profile != null ? $"{b.Profile.FirstName} {b.Profile.LastName}" : "Unknown",
            ProfileFirstName = b.Profile?.FirstName, ProfileMiddleName = b.Profile?.MiddleName, ProfileLastName = b.Profile?.LastName,
            ProfileSex = b.Profile?.Sex, ProfileContact = b.Profile?.ContactNumber, ProfileBirthDate = b.Profile?.BirthDate?.ToString("yyyy-MM-dd"),
            ProfileStatus = b.Profile?.ProfileStatus,
            b.Occupation, b.Venue,
            ImplementationDate = b.ImplementationDate.HasValue ? b.ImplementationDate.Value.ToString("yyyy-MM-dd") : null,
            StartTime = b.StartTime.HasValue ? b.StartTime.Value.ToString(@"hh\:mm\:ss") : null,
            EndTime = b.EndTime.HasValue ? b.EndTime.Value.ToString(@"hh\:mm\:ss") : null,
            b.Status, b.RequesterId, b.AssignedToId, b.AppointmentId
        });
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateBaptism([FromBody] BaptismCreateDto dto)
    {
        var baptism = new Baptism
        {
            RequesterId = dto.RequesterId,
            AssignedToId = dto.AssignedToId,
            ProfileId = dto.ProfileId,
            AppointmentId = dto.AppointmentId,
            Venue = dto.Venue,
            ImplementationDate = dto.ImplementationDate != null ? DateTime.Parse(dto.ImplementationDate) : null,
            StartTime = dto.StartTime != null ? TimeSpan.Parse(dto.StartTime) : null,
            EndTime = dto.EndTime != null ? TimeSpan.Parse(dto.EndTime) : null,
            Occupation = dto.Occupation,
            Status = dto.Status
        };
        _context.Baptisms.Add(baptism);
        await _context.SaveChangesAsync();
        return CreatedAtAction(null, new { id = baptism.BaptismId }, baptism);
    }

    [HttpPut("update/{id}")]
    public async Task<ActionResult> UpdateBaptism(int id, [FromBody] BaptismCreateDto dto)
    {
        var baptism = await _context.Baptisms.FindAsync(id);
        if (baptism == null) return NotFound();
        baptism.RequesterId = dto.RequesterId;
        baptism.AssignedToId = dto.AssignedToId;
        baptism.ProfileId = dto.ProfileId;
        baptism.AppointmentId = dto.AppointmentId;
        baptism.Venue = dto.Venue;
        baptism.ImplementationDate = dto.ImplementationDate != null ? DateTime.Parse(dto.ImplementationDate) : null;
        baptism.StartTime = dto.StartTime != null ? TimeSpan.Parse(dto.StartTime) : null;
        baptism.EndTime = dto.EndTime != null ? TimeSpan.Parse(dto.EndTime) : null;
        baptism.Occupation = dto.Occupation;
        baptism.Status = dto.Status;
        await _context.SaveChangesAsync();
        return Ok(baptism);
    }
}
