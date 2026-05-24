using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using event_service.Models;

namespace event_service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BODEventsController : ControllerBase
{
    private readonly CmisContext _context;

    public BODEventsController(CmisContext context)
    {
        _context = context;
    }

    // GET: api/BODEvents
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BODEvent>>> GetBODEvents()
    {
        return await _context.BODEvents
            .Include(e => e.BODProgramSchedules)
            .Include(e => e.BODGuests)
            .Include(e => e.BODPersonnel)
            .Include(e => e.BODEquipment)
            .Include(e => e.BODTransportation)
            .Include(e => e.BODExpenses)
            .ToListAsync();
    }

    // GET: api/BODEvents/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BODEvent>> GetBODEvent(int id)
    {
        var bodEvent = await _context.BODEvents
            .Include(e => e.BODProgramSchedules)
            .Include(e => e.BODGuests)
            .Include(e => e.BODPersonnel)
            .Include(e => e.BODEquipment)
            .Include(e => e.BODTransportation)
            .Include(e => e.BODExpenses)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (bodEvent == null)
        {
            return NotFound();
        }

        return bodEvent;
    }

    // PUT: api/BODEvents/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBODEvent(int id, BODEvent bodEvent)
    {
        if (id != bodEvent.Id)
        {
            return BadRequest();
        }

        _context.Entry(bodEvent).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BODEventExists(id))
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

    // POST: api/BODEvents
    [HttpPost]
    public async Task<ActionResult<BODEvent>> PostBODEvent(BODEvent bodEvent)
    {
        _context.BODEvents.Add(bodEvent);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetBODEvent", new { id = bodEvent.Id }, bodEvent);
    }

    // DELETE: api/BODEvents/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBODEvent(int id)
    {
        var bodEvent = await _context.BODEvents.FindAsync(id);
        if (bodEvent == null)
        {
            return NotFound();
        }

        _context.BODEvents.Remove(bodEvent);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool BODEventExists(int id)
    {
        return _context.BODEvents.Any(e => e.Id == id);
    }
}
