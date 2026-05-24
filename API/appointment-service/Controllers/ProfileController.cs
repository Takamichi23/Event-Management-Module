using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using appointment_service.Models;
using appointment_service.DTOs;

namespace appointment_service.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfileController : ControllerBase
{
    private readonly CmisContext _context;
    public ProfileController(CmisContext context) { _context = context; }

    // GET /Profile/search?q=juan
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<ProfileReadDto>>> SearchProfiles([FromQuery] string q = "")
    {
        if (string.IsNullOrWhiteSpace(q))
            return Ok(new List<ProfileReadDto>());

        var lower = q.Trim().ToLower();

        var profiles = await _context.Profiles
            .Where(p => (p.FirstName + " " + p.LastName).ToLower().Contains(lower)
                     || p.FirstName.ToLower().Contains(lower)
                     || p.LastName.ToLower().Contains(lower))
            .Take(10)
            .Select(p => new ProfileReadDto
            {
                ProfileId = p.ProfileId,
                FirstName = p.FirstName,
                MiddleName = p.MiddleName,
                LastName = p.LastName,
                Sex = p.Sex,
                ContactNumber = p.ContactNumber,
                Address = p.Address,
                IsMember = p.IsMember,
                BirthDate = p.BirthDate.HasValue ? p.BirthDate.Value.ToString("yyyy-MM-dd") : null,
                ProfileStatus = p.ProfileStatus
            })
            .ToListAsync();

        return Ok(profiles);
    }

    // POST /Profile/create
    [HttpPost("create")]
    public async Task<ActionResult<ProfileReadDto>> CreateProfile([FromBody] ProfileCreateDto dto)
    {
        var profile = new Profile
        {
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            Sex = dto.Sex,
            BirthDate = dto.BirthDate != null ? DateTime.Parse(dto.BirthDate) : null,
            ContactNumber = dto.ContactNumber,
            Address = dto.Address,
            Street = dto.Street,
            Municipality = dto.Municipality,
            Province = dto.Province,
            ChurchId = dto.ChurchId,
            IsMember = dto.IsMember ?? false,
            ProfileStatus = dto.ProfileStatus
        };

        _context.Profiles.Add(profile);
        await _context.SaveChangesAsync();

        return CreatedAtAction(null, new { id = profile.ProfileId }, new ProfileReadDto
        {
            ProfileId = profile.ProfileId,
            FirstName = profile.FirstName,
            MiddleName = profile.MiddleName,
            LastName = profile.LastName,
            Sex = profile.Sex,
            ContactNumber = profile.ContactNumber,
            Address = profile.Address,
            IsMember = profile.IsMember,
            BirthDate = profile.BirthDate.HasValue ? profile.BirthDate.Value.ToString("yyyy-MM-dd") : null,
            ProfileStatus = profile.ProfileStatus
        });
    }
}
