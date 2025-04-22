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
    [Route("api/[controller]")]
    [ApiController]
    public class PressuresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PressuresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Pressures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pressure>>> GetPressures()
        {
            return await _context.Pressure.ToListAsync();
        }

        // GET: api/Pressures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pressure>> GetPressure(int id)
        {
            var pressure = await _context.Pressure.FindAsync(id);

            if (pressure == null)
            {
                return NotFound();
            }

            return pressure;
        }

        // GET: api/Pressures/medical-record/5
        [HttpGet("medical-record/{medicalRecordId}")]
        public async Task<ActionResult<IEnumerable<Pressure>>> GetPressuresByMedicalRecord(int medicalRecordId)
        {
            return await _context.Pressure
                .Where(p => p.MedicalRecordId == medicalRecordId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        // GET: api/Pressures/patient/5
        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<Pressure>>> GetPressuresByPatient(int patientId)
        {
            return await _context.Pressure
                .Where(p => p.PatientId == patientId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }


        // POST: api/Pressures
        [HttpPost]
        public async Task<ActionResult<Pressure>> PostPressure(Pressure pressure)
        {
            // Calculate blood pressure ratio if both systolic and diastolic are provided
            if (pressure.SystolicPressure.HasValue && pressure.DiastolicPressure.HasValue && pressure.DiastolicPressure.Value > 0)
            {
                pressure.BloodPressureRatio = (decimal)pressure.SystolicPressure.Value / pressure.DiastolicPressure.Value;
            }

            // Determine if blood pressure is normal
            // Typically normal blood pressure is considered below 120/80 mmHg
            pressure.IsBloodPressureNormal = pressure.SystolicPressure.HasValue && pressure.DiastolicPressure.HasValue &&
                                           pressure.SystolicPressure.Value < 120 && pressure.DiastolicPressure.Value < 80;

            pressure.CreatedAt = DateTime.UtcNow;
            pressure.UpdatedAt = null;

            _context.Pressure.Add(pressure);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPressure), new { id = pressure.Id }, pressure);
        }

        // PUT: api/Pressures/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPressure(int id, Pressure pressure)
        {
            if (id != pressure.Id)
            {
                return BadRequest();
            }

            // Recalculate blood pressure ratio if both systolic and diastolic are provided
            if (pressure.SystolicPressure.HasValue && pressure.DiastolicPressure.HasValue && pressure.DiastolicPressure.Value > 0)
            {
                pressure.BloodPressureRatio = (decimal)pressure.SystolicPressure.Value / pressure.DiastolicPressure.Value;
            }

            // Update blood pressure normality status
            pressure.IsBloodPressureNormal = pressure.SystolicPressure.HasValue && pressure.DiastolicPressure.HasValue &&
                                           pressure.SystolicPressure.Value < 120 && pressure.DiastolicPressure.Value < 80;

            pressure.UpdatedAt = DateTime.UtcNow;

            _context.Entry(pressure).State = EntityState.Modified;
            // Don't modify CreatedAt when updating
            _context.Entry(pressure).Property(x => x.CreatedAt).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PressureExists(id))
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

        // DELETE: api/Pressures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePressure(int id)
        {
            var pressure = await _context.Pressure.FindAsync(id);
            if (pressure == null)
            {
                return NotFound();
            }

            _context.Pressure.Remove(pressure);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PressureExists(int id)
        {
            return _context.Pressure.Any(e => e.Id == id);
        }
    }
}
