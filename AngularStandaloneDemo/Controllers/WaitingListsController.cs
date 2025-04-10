using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Models;
using AngularStandaloneDemo.Dtos;

namespace AngularStandaloneDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WaitingListController : ControllerBase
    {
        private readonly Data.ApplicationDbContext _context;

        public WaitingListController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/WaitingList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WaitingListDto>>> GetWaitingList()
        {
            var waitingList = await _context.WaitingList
                .AsNoTracking()
                .Include(w => w.Patient)
                .Include(w => w.Provider)
                .Where(w => w.Status != null)
                .ToListAsync();

            return waitingList.Select(MapToDto).ToList();
        }

        // GET: api/WaitingList/provider/{providerId}
        [HttpGet("provider/{providerId}")]
        public async Task<ActionResult<IEnumerable<WaitingListDto>>> GetWaitingListByProvider(int providerId)
        {
            var waitingList = await _context.WaitingList
                .AsNoTracking()
                .Include(w => w.Patient)
                .Include(w => w.Provider)
                .Where(w => w.ProviderId == providerId && w.Status != null)
                .ToListAsync();

            return waitingList.Select(MapToDto).ToList();
        }

        // GET: api/WaitingList/patient/{patientId}
        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<WaitingListDto>>> GetWaitingListByPatient(int patientId)
        {
            var waitingList = await _context.WaitingList
                .AsNoTracking()
                .Include(w => w.Patient)
                .Include(w => w.Provider)
                .Where(w => w.PatientId == patientId && w.Status != null)
                .ToListAsync();

            return waitingList.Select(MapToDto).ToList();
        }

        // GET: api/WaitingList/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<WaitingListDto>> GetWaitingListItem(int id)
        {
            var waitingList = await _context.WaitingList
                .AsNoTracking()
                .Include(w => w.Patient)
                .Include(w => w.Provider)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (waitingList == null)
            {
                return NotFound();
            }

            return MapToDto(waitingList);
        }

        // PUT: api/WaitingList/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWaitingList(int id, WaitingList waitingList)
        {
            if (id != waitingList.Id)
            {
                return BadRequest();
            }

            _context.Entry(waitingList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WaitingListExists(id))
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

        // POST: api/WaitingList
        [HttpPost]
        public async Task<ActionResult<WaitingListDto>> CreateWaitingList(WaitingList waitingList)
        {
            _context.WaitingList.Add(waitingList);
            await _context.SaveChangesAsync();

            var newEntry = await _context.WaitingList
                .Include(w => w.Patient)
                .Include(w => w.Provider)
                .FirstOrDefaultAsync(w => w.Id == waitingList.Id);

            return CreatedAtAction(nameof(GetWaitingListItem), new { id = waitingList.Id }, MapToDto(newEntry));
        }

        // DELETE: api/WaitingList/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWaitingList(int id)
        {
            var waitingList = await _context.WaitingList.FindAsync(id);
            if (waitingList == null)
            {
                return NotFound();
            }

            _context.WaitingList.Remove(waitingList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private WaitingListDto MapToDto(WaitingList waitingList) => new WaitingListDto
        {
            Id = waitingList.Id,
            PatientId = waitingList.PatientId,
            PatientFisrtName = waitingList.Patient?.FirstName, // Note: typo preserved as it's in the DTO
            PatientLastName = waitingList.Patient?.LastName,
            ProviderId = waitingList.ProviderId,
            ProviderLastName = waitingList.Provider?.LastName,
            RequestedDate = waitingList.RequestedDate,
            ExpiryDate = waitingList.ExpiryDate,
            Status = waitingList.Status.ToString(),
            Notes = waitingList.Notes
        };

        private bool WaitingListExists(int id)
        {
            return _context.WaitingList.Any(e => e.Id == id);
        }
    }
}