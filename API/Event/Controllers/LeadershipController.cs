using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using event_service.Models;

namespace event_service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeadershipController : ControllerBase
{
    private readonly CmisContext _context;

    public LeadershipController(CmisContext context)
    {
        _context = context;
    }

    // GET: api/Leadership
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Profile>>> GetProfiles()
    {
        return await _context.Profiles.ToListAsync();
    }

    // GET: api/Leadership/NationalLeaders
    [HttpGet("NationalLeaders")]
    public async Task<ActionResult<IEnumerable<Profile>>> GetNationalLeaders()
    {
        // For now, return all profiles. In a real scenario, you'd filter by role or other criteria.
        return await _context.Profiles
            .Include(p => p.LeadershipAssignments)
            .ThenInclude(a => a.Role)
            .ToListAsync();
    }

    // GET: api/Leadership/DistrictLeaders
    [HttpGet("DistrictLeaders")]
    public async Task<ActionResult<IEnumerable<Profile>>> GetDistrictLeaders()
    {
        // For now, return all profiles.
        return await _context.Profiles
            .Include(p => p.LeadershipAssignments)
            .ThenInclude(a => a.Role)
            .ToListAsync();
    }

    [HttpGet("Test")]
    public async Task<IActionResult> Test() 
    {
        var count = await _context.Profiles.CountAsync();
        return Ok($"Leadership API is working! Found {count} profiles in database.");
    }
}
