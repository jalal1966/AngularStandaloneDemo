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
    // LAB RESULTS CONTROLLER
    [ApiController]
    [Route("api/patients/{patientId}/lab-results")]
    public class LabResultsController : ControllerBase
    {
        private readonly Data.ApplicationDbContext _context;

        public LabResultsController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/patients/5/lab-results
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LabResult>>> GetLabResults(int patientId)
        {
            var results = await _context.LabResults
                .Where(r => r.PatientId == patientId)
                .OrderByDescending(r => r.TestDate)
                .ToListAsync();

            return Ok(results);
        }

        // GET: api/patients/5/lab-results/3
        [HttpGet("{id}")]
        public async Task<ActionResult<LabResult>> GetLabResult(int patientId, int id)
        {
            var result = await _context.LabResults
                .FirstOrDefaultAsync(r => r.PatientId == patientId && r.Id == id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/patients/5/lab-results
        [HttpPost]
        public async Task<ActionResult<LabResult>> CreateLabResult(int patientId, LabResult labResult)
        {
            labResult.PatientId = patientId;

            _context.LabResults.Add(labResult);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLabResult), new { patientId, id = labResult.Id }, labResult);
        }

        // PUT: api/patients/5/lab-results/3
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLabResult(int patientId, int id, LabResult labResult)
        {
            if (id != labResult.Id || patientId != labResult.PatientId)
            {
                return BadRequest();
            }

            var existingResult = await _context.LabResults
                .FirstOrDefaultAsync(r => r.Id == id && r.PatientId == patientId);

            if (existingResult == null)
            {
                return NotFound();
            }

            // Update properties
            existingResult.TestDate = labResult.TestDate;
            existingResult.TestName = labResult.TestName;
            existingResult.Result = labResult.Result;
            existingResult.ReferenceRange = labResult.ReferenceRange;
            existingResult.OrderingProvider = labResult.OrderingProvider;
            existingResult.Notes = labResult.Notes;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/patients/5/lab-results/3
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLabResult(int patientId, int id)
        {
            var result = await _context.LabResults
                .FirstOrDefaultAsync(r => r.Id == id && r.PatientId == patientId);

            if (result == null)
            {
                return NotFound();
            }

            _context.LabResults.Remove(result);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
