using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Models;
using AngularStandaloneDemo.Dtos;
using AngularStandaloneDemo.Services;

namespace AngularStandaloneDemo.Controllers
{
    [ApiController]
    [Route("api/patient-tasks")]
    public class PatientTasksController : Controller
    {
        private readonly ApplicationDbContext _context;
       
        public PatientTasksController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/patient-tasks/nurse/{nurseId}
        [HttpGet("nurse/{nurseId}")]
        public async Task<ActionResult<IEnumerable<PatientTask>>> GetTasksByNurse(int nurseId)
        {
            return await _context.PatientTasks
                .Where(t => t.AssignedToNurseId == nurseId)
                .Include(t => t.Patient)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.DueDate)
                .ToListAsync();
        }

        // GET: api/patient-tasks/patient/{patientId}
        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<PatientTask>>> GetTasksByPatient(int patientId)
        {
            return await _context.PatientTasks
                .Where(t => t.PatientId == patientId)
                .Include(t => t.AssignedNurse)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.DueDate)
                .ToListAsync();
        }

        // POST: api/patient-tasks
        [HttpPost]
        public async Task<ActionResult<PatientTask>> CreateTask(PatientTaskCreateDto taskDto)
        {
            var task = new PatientTask
            {
                PatientId = taskDto.PatientId,
                Title = taskDto.Title,
                Description = taskDto.Description,
                Priority = taskDto.Priority,
                Status = Enums.TaskStatus.NotStarted, 
                DueDate = taskDto.DueDate,
                AssignedToNurseId = taskDto.AssignedToNurseId,
                CreatedByNurseId = taskDto.CreatedByNurseId,
                CreatedDate = DateTime.UtcNow,
                IsRecurring = taskDto.IsRecurring,
                RecurringPattern = taskDto.RecurringPattern
            };

            _context.PatientTasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // GET: api/patient-tasks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientTask>> GetTask(int id)
        {
            var task = await _context.PatientTasks
                .Include(t => t.Patient)
                .Include(t => t.AssignedNurse)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // PUT: api/patient-tasks/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, PatientTask task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            task.LastModifiedDate = DateTime.UtcNow;

            if (task.Status == Enums.TaskStatus.Completed && !task.CompletedDate.HasValue)
            {
                task.CompletedDate = DateTime.UtcNow;
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        // PATCH: api/patient-tasks/{id}/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int id, TaskStatusUpdateDto statusDto)
        {
            var task = await _context.PatientTasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            task.Status = ConvertToCustomTaskStatus(statusDto.Status);  // Convert before assigning
            // task.Status = statusDto.Status;
            task.LastModifiedDate = DateTime.UtcNow;

            if (ConvertToCustomTaskStatus(statusDto.Status) == Enums.TaskStatus.Completed)
            {
                task.CompletedDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private Enums.TaskStatus ConvertToCustomTaskStatus(string status)
        {
            throw new NotImplementedException();
        }


        // DELETE: api/patient-tasks/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.PatientTasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.PatientTasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return _context.PatientTasks.Any(e => e.Id == id);
        }
    }
}
