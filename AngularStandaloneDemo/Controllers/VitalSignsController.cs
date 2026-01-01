using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Dtos;
using AngularStandaloneDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularStandaloneDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class VitalSignsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VitalSignsController> _logger;

        public VitalSignsController(ApplicationDbContext context, ILogger<VitalSignsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/VitalSigns
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VitalSignsResponseDto>>> GetAllVitalSigns()
        {
            try
            {
                // Materialize before mapping - EF Core cannot translate MapToResponseDto into SQL
                var vitalSignEntities = await _context.VitalSigns
                    .AsNoTracking()
                    .Include(v => v.Patient)
                    .OrderByDescending(v => v.RecordedAt)
                    .ToListAsync();

                var vitalSigns = vitalSignEntities
                    .Select(v => MapToResponseDto(v))
                    .ToList();

                return Ok(vitalSigns);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all vital signs");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/VitalSigns/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VitalSignsResponseDto>> GetVitalSignsById(int id)
        {
            try
            {
                var vitalSigns = await _context.VitalSigns
                    .Include(v => v.Patient)
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (vitalSigns == null)
                {
                    return NotFound($"Vital signs record with ID {id} not found");
                }

                return Ok(MapToResponseDto(vitalSigns));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vital signs with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/VitalSigns/patient/5
        [HttpGet("patient/{patientId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<VitalSignsResponseDto>>> GetPatientVitalSigns(int patientId)
        {
            try
            {
                var patientExists = await _context.Patients.AnyAsync(p => p.Id == patientId);
                if (!patientExists)
                {
                    return NotFound($"Patient with ID {patientId} not found");
                }

                var vitalSignEntities = await _context.VitalSigns
                    .AsNoTracking()
                    .Include(v => v.Patient)
                    .Where(v => v.PatientId == patientId)
                    .OrderByDescending(v => v.RecordedAt)
                    .ToListAsync();

                var vitalSigns = vitalSignEntities
                    .Select(v => MapToResponseDto(v))
                    .ToList();

                return Ok(vitalSigns);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vital signs for patient {PatientId}", patientId);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/VitalSigns/patient/5/latest
        [HttpGet("patient/{patientId}/latest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VitalSignsResponseDto>> GetLatestVitalSigns(int patientId)
        {
            try
            {
                var patientExists = await _context.Patients.AnyAsync(p => p.Id == patientId);
                if (!patientExists)
                {
                    return NotFound($"Patient with ID {patientId} not found");
                }

                var vitalSigns = await _context.VitalSigns
                    .Include(v => v.Patient)
                    .Where(v => v.PatientId == patientId)
                    .OrderByDescending(v => v.RecordedAt)
                    .FirstOrDefaultAsync();

                if (vitalSigns == null)
                {
                    return NotFound($"No vital signs records found for patient {patientId}");
                }

                return Ok(MapToResponseDto(vitalSigns));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving latest vital signs for patient {PatientId}", patientId);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/VitalSigns/patient/5/range?startDate=2024-01-01&endDate=2024-12-31
        [HttpGet("patient/{patientId}/range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<VitalSignsResponseDto>>> GetVitalSignsByDateRange(
            int patientId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                {
                    return BadRequest("Start date must be before end date");
                }

                var patientExists = await _context.Patients.AnyAsync(p => p.Id == patientId);
                if (!patientExists)
                {
                    return NotFound($"Patient with ID {patientId} not found");
                }

                var vitalSignEntities = await _context.VitalSigns
                    .AsNoTracking()
                    .Include(v => v.Patient)
                    .Where(v => v.PatientId == patientId &&
                               v.RecordedAt >= startDate &&
                               v.RecordedAt <= endDate)
                    .OrderByDescending(v => v.RecordedAt)
                    .ToListAsync();

                var vitalSigns = vitalSignEntities
                    .Select(v => MapToResponseDto(v))
                    .ToList();

                return Ok(vitalSigns);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vital signs for patient {PatientId} in date range", patientId);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/VitalSigns
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VitalSignsResponseDto>> CreateVitalSigns([FromBody] CreateVitalSignsDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var patientExists = await _context.Patients.AnyAsync(p => p.Id == createDto.PatientId);
                if (!patientExists)
                {
                    return NotFound($"Patient with ID {createDto.PatientId} not found");
                }

                var vitalSigns = new VitalSigns
                {
                    PatientId = createDto.PatientId,
                    Temperature = createDto.Temperature,
                    BloodPressureSystolic = createDto.BloodPressureSystolic,
                    BloodPressureDiastolic = createDto.BloodPressureDiastolic,
                    HeartRate = createDto.HeartRate,
                    RespiratoryRate = createDto.RespiratoryRate,
                    OxygenSaturation = createDto.OxygenSaturation,
                    Weight = createDto.Weight,
                    Remarks = createDto.Remarks,
                    RecordedAt = DateTime.Now
                };

                _context.VitalSigns.Add(vitalSigns);
                await _context.SaveChangesAsync();

                // Reload with patient info
                await _context.Entry(vitalSigns)
                    .Reference(v => v.Patient)
                    .LoadAsync();

                var responseDto = MapToResponseDto(vitalSigns);

                return CreatedAtAction(
                    nameof(GetVitalSignsById),
                    new { id = vitalSigns.Id },
                    responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vital signs record");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/VitalSigns/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VitalSignsResponseDto>> UpdateVitalSigns(int id, [FromBody] UpdateVitalSignsDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingVitalSigns = await _context.VitalSigns
                    .Include(v => v.Patient)
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (existingVitalSigns == null)
                {
                    return NotFound($"Vital signs record with ID {id} not found");
                }

                // Update properties
                existingVitalSigns.Temperature = updateDto.Temperature;
                existingVitalSigns.BloodPressureSystolic = updateDto.BloodPressureSystolic;
                existingVitalSigns.BloodPressureDiastolic = updateDto.BloodPressureDiastolic;
                existingVitalSigns.HeartRate = updateDto.HeartRate;
                existingVitalSigns.RespiratoryRate = updateDto.RespiratoryRate;
                existingVitalSigns.OxygenSaturation = updateDto.OxygenSaturation;
                existingVitalSigns.Weight = updateDto.Weight;
                existingVitalSigns.Remarks = updateDto.Remarks;

                await _context.SaveChangesAsync();

                return Ok(MapToResponseDto(existingVitalSigns));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!VitalSignsExists(id))
                {
                    return NotFound($"Vital signs record with ID {id} not found");
                }
                else
                {
                    _logger.LogError(ex, "Concurrency error updating vital signs with ID {Id}", id);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vital signs with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/VitalSigns/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVitalSigns(int id)
        {
            try
            {
                var vitalSigns = await _context.VitalSigns.FindAsync(id);
                if (vitalSigns == null)
                {
                    return NotFound($"Vital signs record with ID {id} not found");
                }

                _context.VitalSigns.Remove(vitalSigns);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vital signs with ID {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        private bool VitalSignsExists(int id)
        {
            return _context.VitalSigns.Any(e => e.Id == id);
        }

        // Helper method to map entity to response DTO
        private VitalSignsResponseDto MapToResponseDto(VitalSigns vitalSigns)
        {
            return new VitalSignsResponseDto
            {
                Id = vitalSigns.Id,
                PatientId = vitalSigns.PatientId,
                PatientName = vitalSigns.Patient != null
                    ? $"{vitalSigns.Patient.FirstName} {vitalSigns.Patient.LastName}"
                    : null,
                Temperature = vitalSigns.Temperature,
                BloodPressureSystolic = vitalSigns.BloodPressureSystolic,
                BloodPressureDiastolic = vitalSigns.BloodPressureDiastolic,
                HeartRate = vitalSigns.HeartRate,
                RespiratoryRate = vitalSigns.RespiratoryRate,
                OxygenSaturation = vitalSigns.OxygenSaturation,
                Weight = vitalSigns.Weight,
                Remarks = vitalSigns.Remarks,
                RecordedAt = vitalSigns.RecordedAt,
                Status = DetermineStatus(vitalSigns)
            };
        }

        // Helper method to determine vital signs status
        private string DetermineStatus(VitalSigns vs)
        {
            if (vs.BloodPressureSystolic >= 140 || vs.BloodPressureDiastolic >= 90)
                return "High BP";

            if (vs.BloodPressureSystolic < 90 || vs.BloodPressureDiastolic < 60)
                return "Low BP";

            if (vs.Temperature > 38)
                return "Fever";

            if (vs.Temperature < 36)
                return "Hypothermia";

            if (vs.OxygenSaturation < 95)
                return "Low O2";

            if (vs.HeartRate > 100)
                return "Tachycardia";

            if (vs.HeartRate < 60)
                return "Bradycardia";

            return "Normal";
        }
    }
}