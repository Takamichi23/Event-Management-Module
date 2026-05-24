using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using appointment_service.Models;
using appointment_service.DTOs;

namespace appointment_service.Controllers;

[ApiController]
[Route("[controller]")]
public class TimeExceptionController : ControllerBase
{
    private readonly CmisContext _context;
    public TimeExceptionController(CmisContext context) { _context = context; }

    [HttpGet("{accountId}")]
    public async Task<ActionResult<IEnumerable<TimeException>>> GetTimeExceptions(int accountId)
    {
        var exceptions = await _context.TimeExceptions
            .Where(t => t.AccountId == accountId)
            .OrderBy(t => t.Date)
            .ThenBy(t => t.StartTime)
            .ToListAsync();
        return Ok(exceptions);
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateTimeException([FromBody] TimeExceptionCreateDto dto)
    {
        var date = DateTime.Parse(dto.Date).Date;
        
        if (date < DateTime.Today)
        {
            return BadRequest("Cannot create a time exception for a past date.");
        }

        var startTime = TimeSpan.Parse(dto.StartTime);
        var endTime = TimeSpan.Parse(dto.EndTime);

        bool conflict = await _context.TimeExceptions.AnyAsync(t =>
            t.AccountId == dto.AccountId &&
            t.Date.Date == date &&
            startTime < t.EndTime && endTime > t.StartTime);

        if (conflict)
        {
            return Conflict("A time exception already exists during this period on this date.");
        }
        var timeException = new TimeException
        {
            AccountId = dto.AccountId,
            Date = DateTime.Parse(dto.Date),
            StartTime = TimeSpan.Parse(dto.StartTime),
            EndTime = TimeSpan.Parse(dto.EndTime)
        };
        _context.TimeExceptions.Add(timeException);
        await _context.SaveChangesAsync();
        return CreatedAtAction(null, new { id = timeException.TimeExceptionId }, timeException);
    }

    [HttpPut("update/{id}")]
    public async Task<ActionResult> UpdateTimeException(int id, [FromBody] TimeExceptionCreateDto dto)
    {
        var timeException = await _context.TimeExceptions.FindAsync(id);
        if (timeException == null) return NotFound();

        var date = DateTime.Parse(dto.Date).Date;

        if (date < DateTime.Today)
        {
            return BadRequest("Cannot update a time exception to a past date.");
        }

        var startTime = TimeSpan.Parse(dto.StartTime);
        var endTime = TimeSpan.Parse(dto.EndTime);

        bool conflict = await _context.TimeExceptions.AnyAsync(t =>
            t.TimeExceptionId != id &&
            t.AccountId == dto.AccountId &&
            t.Date.Date == date &&
            startTime < t.EndTime && endTime > t.StartTime);

        if (conflict)
        {
            return Conflict("A time exception already exists during this period on this date.");
        }

        timeException.AccountId = dto.AccountId;
        timeException.Date = DateTime.Parse(dto.Date);
        timeException.StartTime = TimeSpan.Parse(dto.StartTime);
        timeException.EndTime = TimeSpan.Parse(dto.EndTime);
        await _context.SaveChangesAsync();
        return Ok(timeException);
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteTimeException(int id)
    {
        var timeException = await _context.TimeExceptions.FindAsync(id);
        if (timeException == null) return NotFound();
        _context.TimeExceptions.Remove(timeException);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
