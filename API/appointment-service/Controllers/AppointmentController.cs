using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using appointment_service.Models; 
using appointment_service.DTOs;
namespace appointment_service.Controllers;

[ApiController]
[Route("[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly CmisContext _context;

    public AppointmentController(CmisContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<AppointmentReadDto>>> GetAppointments(int id)
    {
        // 1. Fetch data and JOIN the related tables
        var appointments = await _context.Appointments
                .Include(a => a.Requester)            // Step 1: Join Account
                    .ThenInclude(r => r.Profile)
            .Include(a => a.AssignedTo)
                .ThenInclude(at => at.Profile)   // Step 2: Join Profile from that Account
                    .ThenInclude(p => p.Church)
            .Where(a => a.RequesterId == id)    
            .ToListAsync();

        // 2. Map to DTO
        var appointmentDtos = appointments.Select(a => new AppointmentReadDto
        {
            AppointmentId = a.AppointmentId,
            ServiceType = Enum.Parse<ServiceType>(a.ServiceType.ToString().Replace(" ", ""), true),
            Status = Enum.Parse<AppointmentStatus>(a.Status.ToString().Replace(" ", ""), true),
            Date = a.Date?.ToString("yyyy-MM-dd") ?? string.Empty,
            
            // 3. Use null-checks (?) just to be safe
            RequesterName = a.Requester?.Profile != null 
                ? $"{a.Requester.Profile.FirstName} {a.Requester.Profile.MiddleName} {a.Requester.Profile.LastName}" 
                : "Unknown",
            AssignedToName = a.AssignedTo?.Profile != null
                ? $"{a.AssignedTo.Profile.FirstName} {a.AssignedTo.Profile.MiddleName} {a.AssignedTo.Profile.LastName}"
                : "Unknown",
            ChurchName = a.AssignedTo?.Profile?.Church?.ChurchName ?? "Unknown",
            ChurchAddress = a.AssignedTo?.Profile?.Church?.Address ?? "Unknown",
            StartTime = a.StartTime.HasValue ? DateTime.Today.Add(a.StartTime.Value).ToString("hh:mm tt") : "Unknown",
            EndTime = a.EndTime.HasValue ? DateTime.Today.Add(a.EndTime.Value).ToString("hh:mm tt") : "Unknown"
        });

        return Ok(appointmentDtos);
    }

    [HttpGet("{accountId}/{id}")]
    public async Task<ActionResult<AppointmentReadDto>> GetAppointment(int accountId,int id)
    {
        var appointment = await _context.Appointments
                .Include(a => a.Requester)
                    .ThenInclude(r => r.Profile)
                .Include(a => a.AssignedTo)
                    .ThenInclude(at => at.Profile)
                        .ThenInclude(p => p.Church)
                .FirstOrDefaultAsync(a => a.AppointmentId == id && a.RequesterId == accountId);

        if (appointment == null)
        {
            return NotFound();
        }

        var appointmentDto = new AppointmentReadDto
        {
            AppointmentId = appointment.AppointmentId,
            ServiceType = Enum.Parse<ServiceType>(appointment.ServiceType.ToString().Replace(" ", ""), true),
            Status = Enum.Parse<AppointmentStatus>(appointment.Status.ToString().Replace(" ", ""), true),
            Date = appointment.Date?.ToString("yyyy-MM-dd") ?? string.Empty,
            RequesterName = appointment.Requester?.Profile != null
                ? $"{appointment.Requester.Profile.FirstName} {appointment.Requester.Profile.MiddleName} {appointment.Requester.Profile.LastName}"
                : "Unknown",
            AssignedToName = appointment.AssignedTo?.Profile != null
                ? $"{appointment.AssignedTo.Profile.FirstName} {appointment.AssignedTo.Profile.MiddleName} {appointment.AssignedTo.Profile.LastName}"
                : "Unknown",
            ChurchName = appointment.AssignedTo?.Profile?.Church?.ChurchName ?? "Unknown",
            ChurchAddress = appointment.AssignedTo?.Profile?.Church?.Address ?? "Unknown",
            StartTime = appointment.StartTime.HasValue ? DateTime.Today.Add(appointment.StartTime.Value).ToString("hh:mm tt") : "Unknown",
            EndTime = appointment.EndTime.HasValue ? DateTime.Today.Add(appointment.EndTime.Value).ToString("hh:mm tt") : "Unknown"
        };

        return Ok(appointmentDto);
    }

    // GET appointments assigned to a specific account (for Head Pastor management)
    [HttpGet("assigned/{accountId}")]
    public async Task<ActionResult<IEnumerable<AppointmentReadDto>>> GetAssignedAppointments(int accountId)
    {
        var appointments = await _context.Appointments
                .Include(a => a.Requester)
                    .ThenInclude(r => r.Profile)
                .Include(a => a.AssignedTo)
                    .ThenInclude(at => at.Profile)
                        .ThenInclude(p => p.Church)
                .Where(a => a.AssignedToId == accountId)
                .ToListAsync();

        var appointmentDtos = appointments.Select(a => new AppointmentReadDto
        {
            AppointmentId = a.AppointmentId,
            ServiceType = Enum.Parse<ServiceType>(a.ServiceType.ToString().Replace(" ", ""), true),
            Status = Enum.Parse<AppointmentStatus>(a.Status.ToString().Replace(" ", ""), true),
            Date = a.Date?.ToString("yyyy-MM-dd") ?? string.Empty,
            RequesterName = a.Requester?.Profile != null
                ? $"{a.Requester.Profile.FirstName} {a.Requester.Profile.MiddleName} {a.Requester.Profile.LastName}"
                : "Unknown",
            AssignedToName = a.AssignedTo?.Profile != null
                ? $"{a.AssignedTo.Profile.FirstName} {a.AssignedTo.Profile.MiddleName} {a.AssignedTo.Profile.LastName}"
                : "Unknown",
            ChurchName = a.AssignedTo?.Profile?.Church?.ChurchName ?? "Unknown",
            ChurchAddress = a.AssignedTo?.Profile?.Church?.Address ?? "Unknown",
            StartTime = a.StartTime.HasValue ? DateTime.Today.Add(a.StartTime.Value).ToString("hh:mm tt") : "Unknown",
            EndTime = a.EndTime.HasValue ? DateTime.Today.Add(a.EndTime.Value).ToString("hh:mm tt") : "Unknown"
        });

        return Ok(appointmentDtos);
    }

    [HttpPost("create")]
    public async Task<ActionResult<AppointmentReadDto>> CreateAppointment(AppointmentCreateDto appointmentDto)
    {
        
        var appointment = new Appointment
        {
            RequesterId = appointmentDto.RequesterId,
            AssignedToId = appointmentDto.AssignedToId,
            ServiceType = appointmentDto.ServiceType,
            Status = "pending",
            Date = DateTime.Parse(appointmentDto.Date),
            StartTime = TimeSpan.Parse(appointmentDto.StartTime),
            EndTime = TimeSpan.Parse(appointmentDto.EndTime)

        };
        DateTime.TryParse(appointmentDto.Date, out DateTime parsedDate);
        TimeSpan.TryParse(appointmentDto.StartTime, out TimeSpan parsedStart);
        {
           if(parsedDate < DateTime.Today || (parsedDate == DateTime.Today && parsedStart < DateTime.Now.TimeOfDay))
            {
                return BadRequest(new { message = "Date cannot be in the past." });
            }
        }
        if ( !await CheckSchedule(_context, appointment.AssignedToId, appointment.Date.Value, appointment.StartTime.Value, appointment.EndTime.Value))
        {
            return Conflict(new { message = "Staff member unavailable." });
        }

        try
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            // 1. Check if the error is actually about the Unique Constraint
            // MySQL Error 1062 is the standard code for "Duplicate entry"
            if (ex.InnerException?.Message.Contains("1062") == true || 
                ex.InnerException?.Message.Contains("Duplicate") == true)
            {
                return Conflict(new { message = "This time slot is already booked. Please choose a different time or date." });
            }

            // 2. If it's some other database error, re-throw it or return generic error
            return StatusCode(500, "A database error occurred while saving.");
        }


        var responseDto = new AppointmentReadDto
        {
            AppointmentId = appointment.AppointmentId,
            // Since this is a fresh save, we don't have the "Included" Profiles yet
            // so we just return the IDs or "Pending Load" strings
            RequesterName = "New Request", 
            ServiceType = Enum.Parse<ServiceType>(appointment.ServiceType.ToString().Replace(" ", ""), true),
            Status = Enum.Parse<AppointmentStatus>(appointment.Status.ToString().Replace(" ", ""), true),
            Date = appointment.Date?.ToString("yyyy-MM-dd") ?? string.Empty
        };

        return CreatedAtAction(nameof(GetAppointment), new {  accountId = appointment.RequesterId,id = appointment.AppointmentId }, responseDto);
    }

  [HttpPut("update/{appointmentId}")]
    public async Task<ActionResult> UpdateAppointment(int appointmentId, [FromBody] AppointmentCreateDto appointmentDto)
    {
        // 1. Parsing and basic validation
        if (!DateTime.TryParse(appointmentDto.Date, out DateTime parsedDate))
            return BadRequest(new { message = "Invalid Date format." });

        if (!TimeSpan.TryParse(appointmentDto.StartTime, out TimeSpan parsedStart) || 
            !TimeSpan.TryParse(appointmentDto.EndTime, out TimeSpan parsedEnd))
            return BadRequest(new { message = "Invalid Time format." });

        // 2. Find existing appointment
        var appointment = await _context.Appointments.FindAsync(appointmentId);
        if (appointment == null) return NotFound();
        if(appointment.Status.ToLower() == "cancelled")
        {
            return BadRequest(new { message = "Cannot update a cancelled appointment." });
        }

        string newStatus = appointmentDto.Status.ToLower();

        // 3. Handle Status Changes
        if (newStatus == "confirmed" || newStatus == "cancelled")
        {
            appointment.Status = appointmentDto.Status;
        }
        else if (newStatus == "rescheduled")
        {
            // USE YOUR HELPER HERE! 
            // We check if the staff is actually available on the new date/time
            bool HasConflict = await _context.Appointments
            .AnyAsync(a => a.AssignedToId == appointment.AssignedToId 
                        && a.AppointmentId != appointmentId 
                        && a.Date == parsedDate 
                        && ((parsedStart >= a.StartTime && parsedStart < a.EndTime) || 
                            (parsedEnd > a.StartTime && parsedEnd <= a.EndTime) || 
                            (parsedStart <= a.StartTime && parsedEnd >= a.EndTime)));
            
            if (HasConflict)
            {
                return Conflict(new { message = "Staff member is unavailable or has a conflict at this new time." });
            }

            // Apply changes
            appointment.Status = appointmentDto.Status;
            appointment.Date = parsedDate;
            appointment.StartTime = parsedStart;
            appointment.EndTime = parsedEnd;
        }
        else
        {
            return BadRequest(new { message = "Invalid status. Use Confirmed, Cancelled, or Rescheduled." });
        }

        try
        {
            await _context.SaveChangesAsync();
            // USE Ok() TO TEST FIRST. If this works, your database logic is perfect.
            return Ok(new { message = "Update successful", id = appointment.AppointmentId });
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Database error occurred.");
        }
    }

    private async Task<bool> CheckSchedule(CmisContext _context, int assignedToId, DateTime date, TimeSpan startTime, TimeSpan endTime)
    {
        // This executes the check entirely on the Database side
        bool isWeeklyConfigAvailable = await _context.WeeklyConfigs
            .AnyAsync(wc => wc.AccountId == assignedToId 
                    && wc.Day == (int)date.DayOfWeek 
                    && wc.StartTime == startTime 
                    && wc.EndTime == endTime 
                    && wc.IsActive == true);


        bool HasDayException = await _context.DayExceptions
            .AnyAsync(de => de.AccountId == assignedToId 
                    && de.Date == date);

        bool HasTimeException = await _context.TimeExceptions
            .AnyAsync(te => te.AccountId == assignedToId 
                && te.Date == date  // Assuming you want only active exceptions
                && startTime < te.EndTime   // Request starts before Exception ends
                && endTime > te.StartTime); // Request ends after Exception starts


        if(isWeeklyConfigAvailable && !HasDayException && !HasTimeException)
        {
            return true; // Available based on weekly config and no day exception
        }
        else
        {
            return false; // Not available due to either weekly config or day exception
        }




    }

    [HttpGet("churches")]
    public async Task<ActionResult<IEnumerable<ChurchReadDto>>> GetChurches()
    {
        var churchDtos = await _context.Churches
            // 1. Filter: Only include churches that have at least one account with RoleId 1
            .Where(church => _context.Accounts.Any(acc => acc.Profile.ChurchId == church.ChurchId && acc.RoleId == 1))
            // 2. Map: Create the DTOs for only those churches
            .Select(church => new ChurchReadDto
            {
                ChurchId = church.ChurchId,
                Name = church.ChurchName,
                Address = church.Address,
                AssignedToId = _context.Accounts
                    .Where(acc => acc.Profile.ChurchId == church.ChurchId && acc.RoleId == 1)
                    .Select(acc => acc.AccountId)
                    .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(churchDtos);
    }

    [HttpGet("available-schedules/{accountId}")]
    public async Task<ActionResult<IEnumerable<AvailableDayDto>>> GetAvailableSchedules(int accountId)
    {
        var startDate = DateTime.Today;
        var endDate = startDate.AddDays(21); // 3 weeks

        var weeklyConfigs = await _context.WeeklyConfigs
            .Where(wc => wc.AccountId == accountId && wc.IsActive == true)
            .ToListAsync();

        var dayExceptions = await _context.DayExceptions
            .Where(de => de.AccountId == accountId && de.Date >= startDate && de.Date <= endDate)
            .Select(de => de.Date.Date)
            .ToListAsync();

        var appointments = await _context.Appointments
            .Where(a => a.AssignedToId == accountId && a.Date >= startDate && a.Date <= endDate)
            .ToListAsync();

        var availableDays = new List<AvailableDayDto>();

        for (int i = 0; i < 21; i++)
        {
            var currentDate = startDate.AddDays(i);

            // 1. Skip if the whole day is marked as an exception
            if (dayExceptions.Contains(currentDate.Date)) continue;

            // 2. Find configurations for this specific day of the week
            // (int)DayOfWeek: Sun=0, Mon=1, Tue=2, Wed=3, Thu=4, Fri=5, Sat=6
            var dayOfWeek = (int)currentDate.DayOfWeek;
            var configsForDay = weeklyConfigs.Where(wc => wc.Day == dayOfWeek);

            foreach (var config in configsForDay)
            {
                // 3. Check if an appointment already exists for this slot
                bool isBooked = appointments.Any(a => 
                    a.Date?.Date == currentDate.Date && 
                    a.StartTime < config.EndTime && 
                    a.EndTime > config.StartTime);

                if (!isBooked)
                {
                    availableDays.Add(new AvailableDayDto
                    {
                        Date = currentDate.ToString("yyyy-MM-dd"),
                        StartTime = config.StartTime.ToString(@"hh\:mm"),
                        EndTime = config.EndTime.ToString(@"hh\:mm"),
                        AssignedToId = accountId
                    });
                }
            }
        }

        return Ok(availableDays);
    }
}