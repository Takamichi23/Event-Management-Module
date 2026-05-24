using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using appointment_service.Models;
using appointment_service.DTOs;

namespace appointment_service.Controllers;

[ApiController]
[Route("[controller]")]
public class WeeklyConfigController : ControllerBase
{
    private readonly CmisContext _context;
    public WeeklyConfigController(CmisContext context) { _context = context; }

    [HttpGet("{accountId}")]
    public async Task<ActionResult<IEnumerable<WeeklyConfig>>> GetWeeklyConfigs(int accountId)
    {
        var configs = await _context.WeeklyConfigs
            .Where(c => c.AccountId == accountId)
            .OrderBy(c => c.Day)
            .ThenBy(c => c.StartTime)
            .ToListAsync();
        return Ok(configs);
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateWeeklyConfig([FromBody] WeeklyConfigCreateDto dto)
    {
        var startTime = TimeSpan.Parse(dto.StartTime);
        var endTime = TimeSpan.Parse(dto.EndTime);

        bool conflict = await _context.WeeklyConfigs.AnyAsync(c =>
            c.AccountId == dto.AccountId &&
            c.Day == dto.Day &&
            startTime < c.EndTime && endTime > c.StartTime);

        if (conflict)
        {
            return Conflict("A time slot already exists during this period.");
        }
        var config = new WeeklyConfig
        {
            AccountId = dto.AccountId,
            Day = dto.Day,
            StartTime = TimeSpan.Parse(dto.StartTime),
            EndTime = TimeSpan.Parse(dto.EndTime),
            IsActive = dto.IsActive
        };
        _context.WeeklyConfigs.Add(config);
        await _context.SaveChangesAsync();
        return CreatedAtAction(null, new { id = config.WeeklyConfigId }, config);
    }

    [HttpPut("update/{id}")]
    public async Task<ActionResult> UpdateWeeklyConfig(int id, [FromBody] WeeklyConfigCreateDto dto)
    {
        var config = await _context.WeeklyConfigs.FindAsync(id);
        if (config == null) return NotFound();

        var startTime = TimeSpan.Parse(dto.StartTime);
        var endTime = TimeSpan.Parse(dto.EndTime);

        bool conflict = await _context.WeeklyConfigs.AnyAsync(c =>
            c.WeeklyConfigId != id &&
            c.AccountId == dto.AccountId &&
            c.Day == dto.Day &&
            startTime < c.EndTime && endTime > c.StartTime);

        if (conflict)
        {
            return Conflict("A time slot already exists during this period.");
        }

        config.AccountId = dto.AccountId;
        config.Day = dto.Day;
        config.StartTime = TimeSpan.Parse(dto.StartTime);
        config.EndTime = TimeSpan.Parse(dto.EndTime);
        config.IsActive = dto.IsActive;
        await _context.SaveChangesAsync();
        return Ok(config);
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> DeleteWeeklyConfig(int id)
    {
        var config = await _context.WeeklyConfigs.FindAsync(id);
        if (config == null) return NotFound();
        _context.WeeklyConfigs.Remove(config);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
