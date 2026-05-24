using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using event_service.Models;

namespace event_service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DistrictEventsController : ControllerBase
{
    private readonly CmisContext _context;

    public DistrictEventsController(CmisContext context)
    {
        _context = context;
    }

    // GET: api/DistrictEvents
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DistrictEvent>>> GetDistrictEvents()
    {
        return await _context.DistrictEvents
            .Include(e => e.DistrictProgramSchedules)
            .Include(e => e.DistrictGuests)
            .Include(e => e.DistrictPersonnel)
            .Include(e => e.DistrictEquipment)
            .Include(e => e.DistrictTransportation)
            .Include(e => e.DistrictExpenses)
            .ToListAsync();
    }

    // GET: api/DistrictEvents/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DistrictEvent>> GetDistrictEvent(int id)
    {
        var districtEvent = await _context.DistrictEvents
            .Include(e => e.DistrictProgramSchedules)
            .Include(e => e.DistrictGuests)
            .Include(e => e.DistrictPersonnel)
            .Include(e => e.DistrictEquipment)
            .Include(e => e.DistrictTransportation)
            .Include(e => e.DistrictExpenses)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (districtEvent == null)
        {
            return NotFound();
        }

        return districtEvent;
    }

    // PUT: api/DistrictEvents/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDistrictEvent(int id, DistrictEvent districtEvent)
    {
        if (id != districtEvent.Id)
        {
            return BadRequest();
        }

        _context.Entry(districtEvent).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DistrictEventExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/DistrictEvents
    [HttpPost]
    public async Task<ActionResult<DistrictEvent>> PostDistrictEvent(DistrictEvent districtEvent)
    {
        _context.DistrictEvents.Add(districtEvent);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetDistrictEvent", new { id = districtEvent.Id }, districtEvent);
    }

    // DELETE: api/DistrictEvents/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDistrictEvent(int id)
    {
        var districtEvent = await _context.DistrictEvents.FindAsync(id);
        if (districtEvent == null)
        {
            return NotFound();
        }

        _context.DistrictEvents.Remove(districtEvent);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool DistrictEventExists(int id)
    {
        return _context.DistrictEvents.Any(e => e.Id == id);
    }
}
