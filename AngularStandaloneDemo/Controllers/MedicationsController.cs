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
        [Route("api/patients/{VisitId}/medications")]
        public class MedicationsController : ControllerBase
        {
            private readonly Data.ApplicationDbContext _context;

            public MedicationsController(Data.ApplicationDbContext context)
            {
                _context = context;
            }

            // GET: api/patients/5/medications
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Medication>>> GetMedications(int VisitId)
            {
                var medications = await _context.Medications
                    .Where(m => m.VisitId == VisitId)
                    .Include(m => m.Diagnosis)
                    .ToListAsync();

                return Ok(medications);
            }

            // GET: api/patients/5/medications/active
            [HttpGet("active")]
            public async Task<ActionResult<IEnumerable<Medication>>> GetActiveMedications(int VisitId)
            {
                var medications = await _context.Medications
                    .Where(m => m.VisitId == VisitId && m.IsActive)
                    .Include(m => m.Diagnosis)
                    .ToListAsync();

                return Ok(medications);
            }

            // GET: api/patients/5/medications/3
            [HttpGet("{id}")]
            public async Task<ActionResult<Medication>> GetMedication(int VisitId, int id)
            {
                var medication = await _context.Medications
                    .Include(m => m.Diagnosis)
                    .FirstOrDefaultAsync(m => m.VisitId == VisitId && m.Id == id);

                if (medication == null)
                {
                    return NotFound();
                }

                return Ok(medication);
            }

            // GET: api/patients/5/medications/diagnosis/7
             [HttpGet("diagnosis/{diagnosisId}")]
             public async Task<ActionResult<IEnumerable<Medication>>> GetMedicationsByDiagnosis(int VisitId, int diagnosisId)
             {
                var medications = await _context.Medications
                   .Where(m => m.VisitId == VisitId && m.DiagnosisId == diagnosisId)
                    .Include(m => m.Diagnosis)
                    .ToListAsync();

                 return Ok(medications);
             }

        // POST: api/patients/5/medications
        [HttpPost]
        public async Task<ActionResult<Medication>> CreateMedication(int VisitId, Medication medication)
        {
            medication.VisitId = VisitId;

            // Set IsActive based on EndDate
            medication.IsActive = medication.EndDate == null || medication.EndDate > DateTime.Now;

            // Set audit fields
            medication.CreatedAt = DateTime.UtcNow;

            _context.Medications.Add(medication);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMedication), new { VisitId, id = medication.Id }, medication);
        }

        // PUT: api/patients/5/medications/3
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedication(int VisitId, int id, Medication medication)
        {
            if (id != medication.Id || VisitId != medication.VisitId)
            {
                return BadRequest();
            }

            var existingMedication = await _context.Medications
                .FirstOrDefaultAsync(m => m.Id == id && m.VisitId == VisitId);

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

            // Update prescription-related properties
            existingMedication.DiagnosisId = medication.DiagnosisId;
            existingMedication.Refillable = medication.Refillable;
            existingMedication.RefillCount = medication.RefillCount;
            existingMedication.Instructions = medication.Instructions;
            existingMedication.PrescriptionNotes = medication.PrescriptionNotes;

            // Update IsActive based on EndDate
            existingMedication.IsActive = medication.EndDate == null || medication.EndDate > DateTime.Now;

            // Update audit field
            existingMedication.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/patients/5/medications/3
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedication(int VisitId, int id)
        {
            var medication = await _context.Medications
                .FirstOrDefaultAsync(m => m.Id == id && m.VisitId == VisitId);

            if (medication == null)
            {
                return NotFound();
            }

            _context.Medications.Remove(medication);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/patients/5/medications/3/toggle-active
        [HttpPatch("{id}/toggle-active")]
        public async Task<IActionResult> ToggleMedicationActive(int VisitId, int id)
        {
            var medication = await _context.Medications
                .FirstOrDefaultAsync(m => m.Id == id && m.VisitId == VisitId);

            if (medication == null)
            {
                return NotFound();
            }

            medication.IsActive = !medication.IsActive;
            medication.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/patients/5/medications/refillable
        [HttpGet("refillable")]
        public async Task<ActionResult<IEnumerable<Medication>>> GetRefillableMedications(int VisitId)
        {
            var medications = await _context.Medications
                .Where(m => m.VisitId == VisitId && m.Refillable && m.RefillCount > 0)
                .Include(m => m.Diagnosis)
                .ToListAsync();

            return Ok(medications);
        }
    }
    
}