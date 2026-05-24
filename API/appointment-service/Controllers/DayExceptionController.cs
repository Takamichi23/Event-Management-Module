using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using appointment_service.Models;
using appointment_service.DTOs;

namespace appointment_service.Controllers;

[ApiController]
[Route("[controller]")]
public class DayExceptionController : ControllerBase
{
    private readonly CmisContext _context;
    public DayExceptionController(CmisContext context) { _context = context; }

    [HttpGet("{accountId}")]
    public async Task<ActionResult<IEnumerable<DayException>>> GetDayExceptions(int accountId)
    {
        var exceptions = await _context.DayExceptions
            .Where(d => d.AccountId == accountId)
            .OrderBy(d => d.Date)
            .ToListAsync();
        return Ok(exceptions);
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateDayException([FromBody] DayExceptionCreateDto dto)
    {
        var date = DateTime.Parse(dto.Date).Date;

        if (date < DateTime.Today)
        {
            return BadRequest("Cannot create an exception for a past date.");
        }

        bool conflict = await _context.DayExceptions.AnyAsync(d =>
            d.AccountId == dto.AccountId &&
            d.Date.Date == date);

        if (conflict)
        {
            return Conflict("A day exception already exists for this date.");
        }
        var dayException = new DayException
        {
            AccountId = dto.AccountId,
            Date = DateTime.Parse(dto.Date),
            Title = dto.Title,
            Description = dto.Description
        };
        _context.DayExceptions.Add(dayException);
        await _context.SaveChangesAsync();
        return CreatedAtAction(null, new { id = dayException.DayExceptionId }, dayException);
    }

    [HttpPut("update/{id}")]
    public async Task<ActionResult> UpdateDayException(int id, [FromBody] DayExceptionCreateDto dto)
    {
        var dayException = await _context.DayExceptions.FindAsync(id);
        if (dayException == null) return NotFound();

        var date = DateTime.Parse(dto.Date).Date;

        if (date < DateTime.Today)
        {
            return BadRequest("Cannot update an exception to a past date.");
        }

        bool conflict = await _context.DayExceptions.AnyAsync(d =>
            d.DayExceptionId != id &&
            d.AccountId == dto.AccountId &&
            d.Date.Date == date);

        if (conflict)
        {
            return Conflict("A day exception already exists for this date.");
        }

        dayException.AccountId = dto.AccountId;
        dayException.Date = DateTime.Parse(dto.Date);
        dayException.Title = dto.Title;
        dayException.Description = dto.Description;
        await _context.SaveChangesAsync();
        return Ok(dayException);
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteDayException(int id)
    {
        var dayException = await _context.DayExceptions.FindAsync(id);
        if (dayException == null) return NotFound();
        _context.DayExceptions.Remove(dayException);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
