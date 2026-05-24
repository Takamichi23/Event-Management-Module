using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using appointment_service.Models; 
using appointment_service.DTOs;
namespace appointment_service.Controllers;


[ApiController]
[Route("[controller]")]
public class ScheduleController : ControllerBase
{
    


    private readonly CmisContext _context;

    public ScheduleController(CmisContext context)
    {
        _context = context;
    }


    
}