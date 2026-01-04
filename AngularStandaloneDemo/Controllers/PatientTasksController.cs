using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Dtos;
using AngularStandaloneDemo.Enums;
using AngularStandaloneDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AngularStandaloneDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientTasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatientTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PatientTasks
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PatientTask>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PatientTask>>> GetPatientTasks()
        {
            return await _context.PatientTasks
                .Include(p => p.AssignedToUser)
                .Include(p => p.CreatedByUser)
                .Include(p => p.Patient)
                .ToListAsync();
        }

        // GET: api/PatientTasks/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PatientTask), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientTask>> GetPatientTask(int id)
        {
            var patientTask = await _context.PatientTasks
                .Include(p => p.AssignedToUser)
                .Include(p => p.CreatedByUser)
                .Include(p => p.Patient)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patientTask == null)
            {
                return NotFound();
            }

            return patientTask;
        }

        // GET: api/PatientTasks/patient/5
        [HttpGet("patient/{patientId}")]
        [ProducesResponseType(typeof(IEnumerable<PatientTask>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PatientTask>>> GetPatientTasksByPatientId(int patientId)
        {
            return await _context.PatientTasks
                .Include(p => p.AssignedToUser)
                .Include(p => p.CreatedByUser)
                .Include(p => p.Patient)
                .Where(p => p.PatientId == patientId)
                .ToListAsync();
        }

        // POST: api/PatientTasks
        [HttpPost]
        [ProducesResponseType(typeof(PatientTask), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PatientTask>> CreatePatientTask([FromBody] CreatePatientTaskDto dto)
        {
            // In a real application, you would get the current user ID from the authentication context
            var currentUserId = 1; // TODO: Replace with actual user ID from authentication

            var patientTask = new PatientTask
            {
                PatientId = dto.PatientId,
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = dto.Status,
                DueDate = dto.DueDate,
                AssignedToUserId = dto.AssignedToUserId,
                CreatedByUserId = currentUserId, // Set from current authenticated user
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = null,
                CompletedDate = null,
                IsRecurring = dto.IsRecurring,
                RecurringPattern = dto.RecurringPattern
            };

            _context.PatientTasks.Add(patientTask);
            await _context.SaveChangesAsync();

            // Load related entities for the response
            await _context.Entry(patientTask)
                .Reference(p => p.AssignedToUser)
                .LoadAsync();
            await _context.Entry(patientTask)
                .Reference(p => p.CreatedByUser)
                .LoadAsync();
            await _context.Entry(patientTask)
                .Reference(p => p.Patient)
                .LoadAsync();

            return CreatedAtAction(nameof(GetPatientTask), new { id = patientTask.Id }, patientTask);
        }

        // PUT: api/PatientTasks/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePatientTask(int id, [FromBody] UpdatePatientTaskDto dto)
        {
            var patientTask = await _context.PatientTasks.FindAsync(id);
            if (patientTask == null)
            {
                return NotFound();
            }

            // Update only the allowed fields (don't update PatientId)
            patientTask.Title = dto.Title;
            patientTask.Description = dto.Description;
            patientTask.Priority = dto.Priority;
            patientTask.Status = dto.Status;
            patientTask.DueDate = dto.DueDate;
            patientTask.AssignedToUserId = dto.AssignedToUserId;
            patientTask.IsRecurring = dto.IsRecurring;
            patientTask.RecurringPattern = dto.RecurringPattern;
            patientTask.LastModifiedDate = DateTime.UtcNow;

            // Set CompletedDate if task is being marked as completed
            if (dto.Status == Enums.TaskStatus.Completed && patientTask.CompletedDate == null)
            {
                patientTask.CompletedDate = DateTime.UtcNow;
            }
            else if (patientTask.Status == Enums.TaskStatus.Completed && dto.Status != Enums.TaskStatus.Completed)
            {
                // If changing from completed back to another status, clear CompletedDate
                patientTask.CompletedDate = null;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientTaskExists(id))
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

        // PATCH: api/PatientTasks/5/status
        [HttpPatch("{id}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] TaskStatusUpdateDto statusUpdate)
        {
            var patientTask = await _context.PatientTasks.FindAsync(id);
            if (patientTask == null)
            {
                return NotFound();
            }

            patientTask.Status = statusUpdate.Status;
            patientTask.LastModifiedDate = DateTime.UtcNow;

            // Set CompletedDate if task is being marked as completed
            if (statusUpdate.Status == Enums.TaskStatus.Completed && patientTask.CompletedDate == null)
            {
                patientTask.CompletedDate = DateTime.UtcNow;
            }
            else if (patientTask.Status == Enums.TaskStatus.Completed && statusUpdate.Status != Enums.TaskStatus.Completed)
            {
                // If changing from completed back to another status, clear CompletedDate
                patientTask.CompletedDate = null;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientTaskExists(id))
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

        // DELETE: api/PatientTasks/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePatientTask(int id)
        {
            var patientTask = await _context.PatientTasks.FindAsync(id);
            if (patientTask == null)
            {
                return NotFound();
            }

            _context.PatientTasks.Remove(patientTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientTaskExists(int id)
        {
            return _context.PatientTasks.Any(e => e.Id == id);
        }
    }

   
   

   
}