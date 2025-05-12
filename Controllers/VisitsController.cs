
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkshopVisitApi.Data;
using WorkshopVisitApi.Models;
using WorkshopVisitApi.DTOs;

namespace WorkshopVisitApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitsController : ControllerBase
    {
        private readonly WorkshopDbContext _context;

        public VisitsController(WorkshopDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVisit(int id)
        {
            var visit = await _context.Visits
                .Include(v => v.Client)
                .Include(v => v.Mechanic)
                .Include(v => v.VisitServices)
                    .ThenInclude(vs => vs.Service)
                .FirstOrDefaultAsync(v => v.VisitId == id);

            if (visit == null)
                return NotFound();

            return Ok(new
            {
                date = visit.Date,
                client = new
                {
                    firstName = visit.Client.FirstName,
                    lastName = visit.Client.LastName,
                    dateOfBirth = visit.Client.DateOfBirth
                },
                mechanic = new
                {
                    mechanicId = visit.Mechanic.MechanicId,
                    licenceNumber = visit.Mechanic.LicenceNumber
                },
                visitServices = visit.VisitServices.Select(vs => new
                {
                    name = vs.Service.Name,
                    serviceFee = vs.ServiceFee
                })
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateVisit([FromBody] VisitCreateDto dto)
        {
            if (_context.Visits.Any(v => v.VisitId == dto.VisitId))
                return Conflict("Visit ID already in use.");

            var client = await _context.Clients.FindAsync(dto.ClientId);
            if (client == null)
                return NotFound("Client not found.");

            var mechanic = await _context.Mechanics.FirstOrDefaultAsync(m => m.LicenceNumber == dto.MechanicLicenceNumber);
            if (mechanic == null)
                return NotFound("Mechanic not found.");

            var visitServices = new List<VisitService>();
            foreach (var s in dto.Services)
            {
                var service = await _context.Services.FirstOrDefaultAsync(x => x.Name == s.ServiceName);
                if (service == null)
                    return NotFound($"Service '{s.ServiceName}' not found.");

                visitServices.Add(new VisitService
                {
                    VisitId = dto.VisitId,
                    ServiceId = service.ServiceId,
                    ServiceFee = s.ServiceFee
                });
            }

            var visit = new Visit
            {
                VisitId = dto.VisitId,
                ClientId = dto.ClientId,
                MechanicId = mechanic.MechanicId,
                Date = DateTime.UtcNow,
                VisitServices = visitServices
            };

            await _context.Visits.AddAsync(visit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVisit), new { id = dto.VisitId }, null);
        }
    }
}
