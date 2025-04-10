using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularStandaloneDemo.Models;

namespace AngularStandaloneDemo.Controllers
{
    [ApiController]
    [Route("api/patients/{patientId}/allergies")]
    public class AllergiesController : ControllerBase
    {
        private readonly Data.ApplicationDbContext _context;

        public AllergiesController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/patients/5/allergies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Allergy>>> GetAllergies(int patientId)
        {
            var allergies = await _context.Allergies
                .Where(a => a.PatientId == patientId)
                .ToListAsync();

            return Ok(allergies);
        }

        // GET: api/patients/5/allergies/3
        [HttpGet("{id}")]
        public async Task<ActionResult<Allergy>> GetAllergy(int patientId, int id)
        {
            var allergy = await _context.Allergies
                .FirstOrDefaultAsync(a => a.PatientId == patientId && a.Id == id);

            if (allergy == null)
            {
                return NotFound();
            }

            return Ok(allergy);
        }

        // POST: api/patients/5/allergies
        [HttpPost]
        public async Task<ActionResult<Allergy>> CreateAllergy(int patientId, Allergy allergy)
        {
            allergy.PatientId = patientId;

            _context.Allergies.Add(allergy);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllergy), new { patientId, id = allergy.Id }, allergy);
        }

        // PUT: api/patients/5/allergies/3
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAllergy(int patientId, int id, Allergy allergy)
        {
            if (id != allergy.Id || patientId != allergy.PatientId)
            {
                return BadRequest();
            }

            var existingAllergy = await _context.Allergies
                .FirstOrDefaultAsync(a => a.Id == id && a.PatientId == patientId);

            if (existingAllergy == null)
            {
                return NotFound();
            }

            // Update properties
            existingAllergy.AllergyType = allergy.AllergyType;
            existingAllergy.Name = allergy.Name;
            existingAllergy.Reaction = allergy.Reaction;
            existingAllergy.Severity = allergy.Severity;
            existingAllergy.DateIdentified = allergy.DateIdentified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/patients/5/allergies/3
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAllergy(int patientId, int id)
        {
            var allergy = await _context.Allergies
                .FirstOrDefaultAsync(a => a.Id == id && a.PatientId == patientId);

            if (allergy == null)
            {
                return NotFound();
            }

            _context.Allergies.Remove(allergy);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
