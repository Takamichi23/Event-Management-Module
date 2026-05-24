using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using event_service.Models;

namespace event_service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly CmisContext _context;

    public EventsController(CmisContext context)
    {
        _context = context;
    }

    // GET: api/Events
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
    {
        return await _context.Events
            .Include(e => e.EventEquipments)
            .Include(e => e.EventExpenses)
            .Include(e => e.EventGuests)
            .Include(e => e.EventPersonnel)
            .Include(e => e.EventProgramSchedules)
            .Include(e => e.EventTransportations)
            .ToListAsync();
    }

    // GET: api/Events/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Event>> GetEvent(int id)
    {
        var @event = await _context.Events
            .Include(e => e.EventEquipments)
            .Include(e => e.EventExpenses)
            .Include(e => e.EventGuests)
            .Include(e => e.EventPersonnel)
            .Include(e => e.EventProgramSchedules)
            .Include(e => e.EventTransportations)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (@event == null)
        {
            return NotFound();
        }

        return @event;
    }

    // PUT: api/Events/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEvent(int id, Event @event)
    {
        if (id != @event.Id)
        {
            return BadRequest();
        }

        _context.Entry(@event).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EventExists(id))
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

    // POST: api/Events
    [HttpPost]
    public async Task<ActionResult<Event>> PostEvent(Event @event)
    {
        _context.Events.Add(@event);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetEvent", new { id = @event.Id }, @event);
    }

    // DELETE: api/Events/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var @event = await _context.Events.FindAsync(id);
        if (@event == null)
        {
            return NotFound();
        }

        _context.Events.Remove(@event);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EventExists(int id)
    {
        return _context.Events.Any(e => e.Id == id);
    }
}
