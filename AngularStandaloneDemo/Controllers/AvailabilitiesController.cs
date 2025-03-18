using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Models;
using AngularStandaloneDemo.Enums;
using AngularStandaloneDemo.Supporting;

namespace AngularStandaloneDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvailabilitiesController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Availability>>> GetAvailabilityByUser(int userId)
        {
            return await _context.Availabilities
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Availability>> GetAvailability(int id)
        {
            var availability = await _context.Availabilities.FindAsync(id);

            if (availability == null)
            {
                return NotFound();
            }

            return availability;
        }

        [HttpPost]
        public async Task<ActionResult<Availability>> CreateAvailability(Availability availability)
        {
            _context.Availabilities.Add(availability);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAvailability), new { id = availability.Id }, availability);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAvailability(int id, Availability availability)
        {
            if (id != availability.Id)
            {
                return BadRequest();
            }

            _context.Entry(availability).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AvailabilityExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvailability(int id)
        {
            var availability = await _context.Availabilities.FindAsync(id);
            if (availability == null)
            {
                return NotFound();
            }

            _context.Availabilities.Remove(availability);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("calendar/{userId}")]
        public async Task<ActionResult<Dictionary<DateTime, List<TimeSlot>>>> GetAvailabilityCalendar(
            int userId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            if (endDate < startDate)
            {
                return BadRequest("End date must be after start date");
            }

            // Get all availabilities for the user
            var availabilities = await _context.Availabilities
                .Where(a => a.UserId == userId)
                .ToListAsync();

            // Get all appointments for the user in the date range
            var appointments = await _context.Appointments
                .Where(a => a.ProviderId == userId &&
                       a.StartTime >= startDate &&
                       a.EndTime <= endDate &&
                       a.Status != AppointmentStatus.Cancelled)
                .ToListAsync();

            // Calculate available slots by date
            var calendarDays = new Dictionary<DateTime, List<TimeSlot>>();

            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                var availableSlots = new List<TimeSlot>();

                // Get applicable availabilities for this date
                foreach (var availability in availabilities)
                {
                    if (IsAvailabilityApplicableForDate(availability, date))
                    {
                        // Get start and end times for this specific date
                        var dayStart = new DateTime(date.Year, date.Month, date.Day,
                            availability.StartTime.Hour, availability.StartTime.Minute, 0);

                        var dayEnd = new DateTime(date.Year, date.Month, date.Day,
                            availability.EndTime.Hour, availability.EndTime.Minute, 0);

                        // Add all 30-minute slots
                        for (var time = dayStart; time.AddMinutes(30) <= dayEnd; time = time.AddMinutes(30))
                        {
                            bool isBooked = appointments.Any(a =>
                                a.StartTime <= time && a.EndTime > time);

                            availableSlots.Add(new TimeSlot
                            {
                                StartTime = time,
                                EndTime = time.AddMinutes(30),
                                IsAvailable = !isBooked
                            });
                        }
                    }
                }

                calendarDays.Add(date, availableSlots);
            }

            return calendarDays;
        }

        private bool IsAvailabilityApplicableForDate(Availability availability, DateTime date)
        {
            // For non-recurring availabilities, check if the date is within the range
            if (!availability.IsRecurring)
            {
                return availability.StartTime.Date <= date && availability.EndTime.Date >= date;
            }

            // For recurring availabilities, check the pattern
            switch (availability.RecurrencePattern)
            {
                case RecurrencePattern.Daily:
                    return true;

                case RecurrencePattern.Weekly:
                    return availability.StartTime.DayOfWeek == date.DayOfWeek;

                case RecurrencePattern.BiWeekly:
                    // Calculate week number difference
                    int weeksDiff = (int)Math.Floor((date - availability.StartTime).TotalDays / 7);
                    return availability.StartTime.DayOfWeek == date.DayOfWeek && weeksDiff % 2 == 0;

                case RecurrencePattern.Monthly:
                    return availability.StartTime.Day == date.Day;

                default:
                    return false;
            }
        }

        private bool AvailabilityExists(int id)
        {
            return _context.Availabilities.Any(e => e.Id == id);
        }
    }

    // --- UserController for doctors and nurses ---
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("providers")]
        public async Task<ActionResult<IEnumerable<User>>> GetProviders()
        {
            return await _context.Users
                .Where(u => u.Role == UserRole.Doctor || u.Role == UserRole.Nurse)
                .ToListAsync();
        }

        [HttpGet("doctors")]
        public async Task<ActionResult<IEnumerable<User>>> GetDoctors()
        {
            return await _context.Users
                .Where(u => u.Role == UserRole.Doctor)
                .ToListAsync();
        }

        [HttpGet("nurses")]
        public async Task<ActionResult<IEnumerable<User>>> GetNurses()
        {
            return await _context.Users
                .Where(u => u.Role == UserRole.Nurse)
                .ToListAsync();
        }

        [HttpGet("patients")]
        public async Task<ActionResult<IEnumerable<User>>> GetPatients()
        {
            return await _context.Users
                .Where(u => u.Role == UserRole.Patient)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.UserID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}
