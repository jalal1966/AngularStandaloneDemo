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
        [Route("api/patients/{patientId}/medications")]
        public class MedicationsController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public MedicationsController(ApplicationDbContext context)
            {
                _context = context;
            }

            // GET: api/patients/5/medications
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Medication>>> GetMedications(int patientId)
            {
                var medications = await _context.Medications
                    .Where(m => m.PatientId == patientId)
                    .ToListAsync();

                return Ok(medications);
            }

            // GET: api/patients/5/medications/active
            [HttpGet("active")]
            public async Task<ActionResult<IEnumerable<Medication>>> GetActiveMedications(int patientId)
            {
                var medications = await _context.Medications
                    .Where(m => m.PatientId == patientId && m.IsActive)
                    .ToListAsync();

                return Ok(medications);
            }

            // GET: api/patients/5/medications/3
            [HttpGet("{id}")]
            public async Task<ActionResult<Medication>> GetMedication(int patientId, int id)
            {
                var medication = await _context.Medications
                    .FirstOrDefaultAsync(m => m.PatientId == patientId && m.Id == id);

                if (medication == null)
                {
                    return NotFound();
                }

                return Ok(medication);
            }

            // POST: api/patients/5/medications
            [HttpPost]
            public async Task<ActionResult<Medication>> CreateMedication(int patientId, Medication medication)
            {
                medication.PatientId = patientId;

                // Set IsActive based on EndDate
                medication.IsActive = medication.EndDate == null || medication.EndDate > DateTime.Now;

                _context.Medications.Add(medication);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetMedication), new { patientId, id = medication.Id }, medication);
            }

            // PUT: api/patients/5/medications/3
            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateMedication(int patientId, int id, Medication medication)
            {
                if (id != medication.Id || patientId != medication.PatientId)
                {
                    return BadRequest();
                }

                var existingMedication = await _context.Medications
                    .FirstOrDefaultAsync(m => m.Id == id && m.PatientId == patientId);

                if (existingMedication == null)
                {
                    return NotFound();
                }

                // Update properties
                existingMedication.Name = medication.Name;
                existingMedication.Dosage = medication.Dosage;
                existingMedication.Frequency = medication.Frequency;
                existingMedication.StartDate = medication.StartDate;
                existingMedication.EndDate = medication.EndDate;
                existingMedication.PrescribingProvider = medication.PrescribingProvider;
                existingMedication.Purpose = medication.Purpose;

                // Update IsActive based on EndDate
                existingMedication.IsActive = medication.EndDate == null || medication.EndDate > DateTime.Now;

                await _context.SaveChangesAsync();

                return NoContent();
            }

            // DELETE: api/patients/5/medications/3
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteMedication(int patientId, int id)
            {
                var medication = await _context.Medications
                    .FirstOrDefaultAsync(m => m.Id == id && m.PatientId == patientId);

                if (medication == null)
                {
                    return NotFound();
                }

                _context.Medications.Remove(medication);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    
}
