using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Models;

namespace AngularStandaloneDemo.Controllers
{
    [ApiController]
    [Route("api/patients/{patientId}/immunizations")]
    public class ImmunizationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ImmunizationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/patients/5/immunizations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Immunization>>> GetImmunizations(int patientId)
        {
            var immunizations = await _context.Immunizations
                .Where(i => i.PatientId == patientId)
                .OrderByDescending(i => i.AdministrationDate)
                .ToListAsync();

            return Ok(immunizations);
        }

        // GET: api/patients/5/immunizations/3
        [HttpGet("{id}")]
        public async Task<ActionResult<Immunization>> GetImmunization(int patientId, int id)
        {
            var immunization = await _context.Immunizations
                .FirstOrDefaultAsync(i => i.PatientId == patientId && i.Id == id);

            if (immunization == null)
            {
                return NotFound();
            }

            return Ok(immunization);
        }

        // POST: api/patients/5/immunizations
        [HttpPost]
        public async Task<ActionResult<Immunization>> CreateImmunization(int patientId, Immunization immunization)
        {
            immunization.PatientId = patientId;

            _context.Immunizations.Add(immunization);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImmunization), new { patientId, id = immunization.Id }, immunization);
        }

        // PUT: api/patients/5/immunizations/3
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateImmunization(int patientId, int id, Immunization immunization)
        {
            if (id != immunization.Id || patientId != immunization.PatientId)
            {
                return BadRequest();
            }

            var existingImmunization = await _context.Immunizations
                .FirstOrDefaultAsync(i => i.Id == id && i.PatientId == patientId);

            if (existingImmunization == null)
            {
                return NotFound();
            }

            // Update properties
            existingImmunization.VaccineName = immunization.VaccineName;
            existingImmunization.AdministrationDate = immunization.AdministrationDate;
            existingImmunization.LotNumber = immunization.LotNumber;
            existingImmunization.AdministeringProvider = immunization.AdministeringProvider;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/patients/5/immunizations/3
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImmunization(int patientId, int id)
        {
            var immunization = await _context.Immunizations
                .FirstOrDefaultAsync(i => i.Id == id && i.PatientId == patientId);

            if (immunization == null)
            {
                return NotFound();
            }

            _context.Immunizations.Remove(immunization);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
