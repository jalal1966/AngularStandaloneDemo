using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularStandaloneDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitsController : ControllerBase
    {
        private readonly Data.ApplicationDbContext _context;

        public VisitsController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Visits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Visit>>> GetVisits() => await _context.Visits
                .Include(v => v.PatientId)
                .Include(v => v.Diagnoses)
                .ToListAsync();

        // GET: api/Visits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Visit>> GetVisit(int id)
        {
            var visit = await _context.Visits
                .Include(v => v.PatientId)
                .Include(v => v.Diagnoses)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (visit == null)
            {
                return NotFound();
            }

            return visit;
        }

        // GET: api/Visits/Patient/5
        [HttpGet("Patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<Visit>>> GetVisitsByPatient(int patientId)
        {
            return await _context.Visits
                .Where(v => v.PatientId == patientId)
                .Include(v => v.Diagnoses)
                .ToListAsync();
        }

        // POST: api/Visits
        [HttpPost]
        public async Task<ActionResult<Visit>> CreateVisit(Visit visit)
        {
            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVisit), new { id = visit.Id }, visit);
        }

        // PUT: api/Visits/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVisit(int id, Visit visit)
        {
            if (id != visit.Id)
            {
                return BadRequest();
            }

            _context.Entry(visit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisitExists(id))
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

        // DELETE: api/Visits/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisit(int id)
        {
            var visit = await _context.Visits.FindAsync(id);
            if (visit == null)
            {
                return NotFound();
            }

            _context.Visits.Remove(visit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VisitExists(int id)
        {
            return _context.Visits.Any(e => e.Id == id);
        }
    }
}