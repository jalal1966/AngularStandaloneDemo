using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Dtos;
using AngularStandaloneDemo.Enums;
using DoctorAppointmentSystem.DTOs;
using DoctorAppointmentSystem.Models;
using AngularStandaloneDemo.Models;
using System.Data;
namespace AngularStandaloneDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AppointmentsController> _logger; // If you want logging

        // Single constructor with all required dependencies
        public AppointmentsController(
            ApplicationDbContext context,
            ILogger<AppointmentsController> logger = null) // Make logger optional if not all controllers use it
        {
            _context = context;
            _logger = logger;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointments()
        {
            var dateNow = DateTime.UtcNow.Date; // Define inside method
            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Provider)
                .Where(a => a.StartTime >= dateNow) // Filtering in the database
                .ToListAsync();

            return appointments.Select(MapToDto).ToList() ;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetAppointment(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Provider)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return MapToDto(appointment);
        }

        [HttpGet("provider/{providerId}")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointmentsByProvider(int providerId)
        {
            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.ProviderId == providerId)
                .ToListAsync();

            return appointments.Select(MapToDto).ToList();
        }


        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointmentsByPatient(int patientId)
        {
            var appointments = await _context.Appointments
                .Include(a => a.Provider)
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
                

            return appointments.Select(MapToDto).ToList();
        }

        [HttpGet("available-slots")]
        public async Task<ActionResult<IEnumerable<DateTime>>> GetAvailableSlots(
            [FromQuery] int providerId,
            [FromQuery] DateTime date)
        {
            // Get provider's availability for the specified date
            var availabilities = await _context.Availabilities
                .Where(a => a.UserId == providerId &&
                       a.StartTime.Date <= date.Date &&
                       (a.EndTime.Date >= date.Date || a.IsRecurring))
                .ToListAsync();

            if (availabilities.Count == 0)
            {
                return new List<DateTime>();
            }

            // Get booked appointments for the provider on that date
            var bookedAppointments = await _context.Appointments
                .Where(a => a.ProviderId == providerId &&
                       a.StartTime.Date == date.Date &&
                       a.Status != AppointmentStatus.Cancelled)
                .ToListAsync();

            // Calculate available slots (30-minute intervals)
            var availableSlots = new List<DateTime>();
            foreach (var availability in availabilities)
            {
                DateTime startTime = availability.StartTime;
                if (startTime.Date < date.Date)
                {
                    startTime = new DateTime(date.Year, date.Month, date.Day,
                                            availability.StartTime.Hour,
                                            availability.StartTime.Minute, 0);
                }

                DateTime endTime = availability.EndTime;
                if (endTime.Date > date.Date)
                {
                    endTime = new DateTime(date.Year, date.Month, date.Day,
                                          23, 59, 59);
                }

                for (DateTime slot = startTime; slot.AddMinutes(30) <= endTime; slot = slot.AddMinutes(30))
                {
                    if (!bookedAppointments.Any(a =>
                        (slot >= a.StartTime && slot < a.EndTime) ||
                        (slot.AddMinutes(30) > a.StartTime && slot.AddMinutes(30) <= a.EndTime)))
                    {
                        availableSlots.Add(slot);
                    }
                }
            }

            return availableSlots;
        }


        [HttpGet("doctr-waiting-list")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointmentsByDateRange(
    [FromQuery] DateTime startDate,
    [FromQuery] DateTime endDate,
    [FromQuery] int? providerId = null)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date must be before or equal to end date.");
            }

            // Start with base query
            var query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Provider)
                .Where(a => a.StartTime >= startDate && a.StartTime <= endDate && a.ProviderId==providerId);

            // Apply optional filters
            if (providerId.HasValue)
            {
                query = query.Where(a => a.ProviderId == providerId.Value);
            }

            var appointments = await query.ToListAsync();
            return appointments.Select(MapToDto).ToList();
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointmentsByDateRange(
          [FromQuery] DateTime startDate,
          [FromQuery] DateTime endDate)
          
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date must be before or equal to end date.");
            }

            // Start with base query
            var query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Provider)
                .Where(a => a.StartTime >= startDate && a.StartTime <= endDate);


            var appointments = await query.ToListAsync();
            return appointments.Select(MapToDto).ToList();
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> CreateAppointment(AppointmentCreateDto appointmentDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Validate appointment duration (e.g., minimum 15 minutes, maximum 2 hours)
                if (appointmentDto.EndTime - appointmentDto.StartTime < TimeSpan.FromMinutes(15) ||
                    appointmentDto.EndTime - appointmentDto.StartTime > TimeSpan.FromHours(2))
                {
                    return BadRequest("Invalid appointment duration.");
                }

                // More comprehensive conflicting appointment check
                var conflictingAppointment = await _context.Appointments
                    .AnyAsync(a => a.ProviderId == appointmentDto.ProviderId &&
                               a.Status != AppointmentStatus.Cancelled &&
                               a.StartTime < appointmentDto.EndTime &&
                               a.EndTime > appointmentDto.StartTime);

                if (conflictingAppointment)
                {
                    return BadRequest("The requested time slot is not available.");
                }

                // Verify provider and patient with more efficient querying
                var provider = await _context.Users
                    .FirstOrDefaultAsync(p => p.UserID == appointmentDto.ProviderId);

                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.Id == appointmentDto.PatientId);

                if (provider == null || patient == null)
                {
                    return NotFound("Provider or Patient not found.");
                }

                var appointment = new Appointment
                {
                    PatientId = appointmentDto.PatientId,
                    Patient = patient,
                    ProviderId = appointmentDto.ProviderId,
                    Provider = provider,
                    StartTime = appointmentDto.StartTime,
                    EndTime = appointmentDto.EndTime,
                    Type = Enum.Parse<AppointmentType>(appointmentDto.Type),
                    Status = Enum.Parse<AppointmentStatus>(appointmentDto.Status),
                    Notes = appointmentDto.Notes,
                };

                // Update patient's last visit date
                if (appointment.Status is AppointmentStatus.Completed or AppointmentStatus.Scheduled)
                {
                    patient.LastVisitDate = appointment.StartTime;
                }

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                // Handle waiting list
                var waitingRequest = await _context.WaitingList
                    .FirstOrDefaultAsync(w => w.PatientId == appointment.PatientId &&
                                       w.ProviderId == appointment.ProviderId &&
                                       w.Status == WaitingStatus.Active);

                if (waitingRequest != null)
                {
                    waitingRequest.Status = WaitingStatus.Fulfilled;
                    await _context.SaveChangesAsync();
                }

                // Commit transaction
                await transaction.CommitAsync();

                // Reload appointment with related entities
                appointment = await _context.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Provider)
                    .FirstOrDefaultAsync(a => a.Id == appointment.Id);

                return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, MapToDto(appointment));
            }
            catch (Exception ex)
            {
                // Rollback transaction
                await transaction.RollbackAsync();
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, AppointmentUpdateDto appointmentDto)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            // Update appointment with DTO values
            appointment.StartTime = appointmentDto.StartTime;
            appointment.EndTime = appointmentDto.EndTime;
            appointment.Notes = appointmentDto.Notes;
            appointment.Type = Enum.Parse<AppointmentType>(appointmentDto.Type);
            appointment.Status = Enum.Parse<AppointmentStatus>(appointmentDto.Status);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
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

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateAppointmentStatus(int id, [FromBody] string status)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            // Ensure the status string is valid for the enum
            if (!Enum.TryParse<AppointmentStatus>(status, out var parsedStatus))
            {
                return BadRequest("Invalid status value.");
            }
            appointment.Status = parsedStatus;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            // Soft delete by changing status to Cancelled
            appointment.Status = AppointmentStatus.Cancelled;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }

        private AppointmentDto MapToDto(Appointment appointment)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            return new AppointmentDto
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                PatientFirstName = appointment.Patient?.FirstName,
                PatientLastName = appointment.Patient?.LastName,
                ProviderId = appointment.ProviderId,
                ProviderFirstName = appointment.Provider?.FirstName,
                ProviderLastName = appointment.Provider?.LastName,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Type = appointment.Type.ToString(),
                Status = appointment.Status.ToString(),
                Notes = appointment.Notes,
            };
#pragma warning restore CS8601 // Possible null reference assignment.
        }
    }

    // --- Updated WaitingListController ---
    [ApiController]
    [Route("api/[controller]")]
    public class WaitingListController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WaitingListController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WaitingListDto>>> GetWaitingList()
        {
            var waitingList = await _context.WaitingList
                 .AsNoTracking()
                .Include(w => w.Patient)
                .Include(w => w.Provider)
                .Where(predicate: static w => w.Status != null)
                .ToListAsync();
            // var dtos = waitingList.Select(MapToDto).ToList();
            return waitingList.Select(MapToDto).ToList();
        }


        [HttpGet("provider/{providerId}")]
        public async Task<ActionResult<IEnumerable<WaitingListDto>>> GetWaitingListByProvider(int providerId)
        {
            var waitingList = await _context.WaitingList
                .Include(w => w.Patient)
                .Where(w => w.ProviderId == providerId && w.Status != null)
                .ToListAsync();

            return waitingList.Select(MapToDto).ToList();
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<WaitingListDto>>> GetWaitingListByPatient(int patientId)
        {
            var waitingList = await _context.WaitingList
                .Include(w => w.Provider)
                .Where(w => w.PatientId == patientId && w.Status != null)
                .ToListAsync();

            return waitingList.Select(MapToDto).ToList();
        }

        // Other methods remain the same...

        private WaitingListDto MapToDto(WaitingList waitingList) => new WaitingListDto
        {
            Id = waitingList.Id,
            PatientId = waitingList.PatientId,
            PatientFisrtName = waitingList.Patient?.FirstName,
            PatientLastName = waitingList.Patient?.LastName,
            ProviderId = waitingList.ProviderId,
            ProviderLastName = waitingList.Provider?.LastName,
            RequestedDate = waitingList.RequestedDate,
            ExpiryDate = waitingList.ExpiryDate,
            Status = waitingList.Status.ToString(),
            Notes = waitingList.Notes
        };
    }

    internal record NewRecord(int Id);
}