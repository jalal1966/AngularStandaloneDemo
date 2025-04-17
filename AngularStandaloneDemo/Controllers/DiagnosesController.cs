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
    public class DiagnosesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DiagnosesController> _logger;

        public DiagnosesController(ApplicationDbContext context, ILogger<DiagnosesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Diagnoses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Diagnosis>>> GetDiagnoses()
        {
            try
            {
                return await _context.Diagnosis
                    .Include(d => d.Visit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving diagnoses");
                return StatusCode(500, "An error occurred while retrieving diagnoses");
            }
        }

        // GET: api/Diagnoses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Diagnosis>> GetDiagnosis(int id)
        {
            try
            {
                var diagnosis = await _context.Diagnosis
                    .Include(d => d.Visit)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (diagnosis == null)
                {
                    return NotFound($"Diagnosis with ID {id} not found");
                }

                return diagnosis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving diagnosis with ID {Id}", id);
                return StatusCode(500, $"An error occurred while retrieving diagnosis with ID {id}");
            }
        }

        // GET: api/Diagnoses/ByVisit/5
        [HttpGet("ByVisit/{visitId}")]
        public async Task<ActionResult<IEnumerable<Diagnosis>>> GetDiagnosesByVisit(int visitId)
        {
            try
            {
                var diagnoses = await _context.Diagnosis
                    .Where(d => d.VisitId == visitId)
                    .ToListAsync();

                if (!diagnoses.Any())
                {
                    return NotFound($"No diagnoses found for visit ID {visitId}");
                }

                return diagnoses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving diagnoses for visit ID {VisitId}", visitId);
                return StatusCode(500, $"An error occurred while retrieving diagnoses for visit ID {visitId}");
            }
        }

        // POST: api/Diagnoses
        [HttpPost]
        public async Task<ActionResult<Diagnosis>> CreateDiagnosis(Diagnosis diagnosis)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Set audit fields
                diagnosis.CreatedAt = DateTime.UtcNow;

                _context.Diagnosis.Add(diagnosis);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetDiagnosis), new { id = diagnosis.Id }, diagnosis);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating diagnosis");
                return StatusCode(500, "An error occurred while creating the diagnosis");
            }
        }

        // PUT: api/Diagnoses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiagnosis(int id, Diagnosis diagnosis)
        {
            if (id != diagnosis.Id)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Update audit field
                diagnosis.UpdatedAt = DateTime.UtcNow;

                _context.Entry(diagnosis).State = EntityState.Modified;
                // Don't modify creation date
                _context.Entry(diagnosis).Property(x => x.CreatedAt).IsModified = false;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiagnosisExists(id))
                    {
                        return NotFound($"Diagnosis with ID {id} not found");
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating diagnosis with ID {Id}", id);
                return StatusCode(500, $"An error occurred while updating the diagnosis with ID {id}");
            }
        }

        // PUT: api/Diagnoses/PartialUpdate/5
        [HttpPut("PartialUpdate/{id}")]
        public async Task<IActionResult> PartialUpdateDiagnosis(int id, [FromBody] Dictionary<string, object> updates)
        {
            try
            {
                var diagnosis = await _context.Diagnosis.FindAsync(id);

                if (diagnosis == null)
                {
                    return NotFound($"Diagnosis with ID {id} not found");
                }

                // Apply updates
                foreach (var item in updates)
                {
                    var propertyInfo = typeof(Diagnosis).GetProperty(item.Key);
                    if (propertyInfo != null && propertyInfo.CanWrite)
                    {
                        // Handle different property types
                        try
                        {
                            object convertedValue = Convert.ChangeType(item.Value, propertyInfo.PropertyType);
                            propertyInfo.SetValue(diagnosis, convertedValue);
                        }
                        catch
                        {
                            return BadRequest($"Cannot convert value for property {item.Key}");
                        }
                    }
                    else
                    {
                        return BadRequest($"Property {item.Key} does not exist or is read-only");
                    }
                }

                // Update audit field
                diagnosis.UpdatedAt = DateTime.UtcNow;

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating diagnosis with ID {Id}", id);
                return StatusCode(500, $"An error occurred while updating the diagnosis with ID {id}");
            }
        }

        // DELETE: api/Diagnoses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiagnosis(int id)
        {
            try
            {
                var diagnosis = await _context.Diagnosis.FindAsync(id);
                if (diagnosis == null)
                {
                    return NotFound($"Diagnosis with ID {id} not found");
                }

                _context.Diagnosis.Remove(diagnosis);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting diagnosis with ID {Id}", id);
                return StatusCode(500, $"An error occurred while deleting the diagnosis with ID {id}");
            }
        }

        // GET: api/Diagnoses/ActiveOnly
        [HttpGet("ActiveOnly")]
        public async Task<ActionResult<IEnumerable<Diagnosis>>> GetActiveDiagnoses()
        {
            try
            {
                return await _context.Diagnosis
                    .Where(d => d.IsActive)
                    .Include(d => d.Visit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active diagnoses");
                return StatusCode(500, "An error occurred while retrieving active diagnoses");
            }
        }

        // GET: api/Diagnoses/FollowUpNeeded
        [HttpGet("FollowUpNeeded")]
        public async Task<ActionResult<IEnumerable<Diagnosis>>> GetFollowUpDiagnoses()
        {
            try
            {
                return await _context.Diagnosis
                    .Where(d => d.FollowUpNeeded && d.FollowUpDate.HasValue)
                    .Include(d => d.Visit)
                    .OrderBy(d => d.FollowUpDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving diagnoses needing follow-up");
                return StatusCode(500, "An error occurred while retrieving diagnoses needing follow-up");
            }
        }

        private bool DiagnosisExists(int id)
        {
            return _context.Diagnosis.Any(e => e.Id == id);
        }
    }
}