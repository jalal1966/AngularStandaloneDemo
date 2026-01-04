using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Dtos;
using AngularStandaloneDemo.Models;
using AngularStandaloneDemo.Enums;
using Microsoft.EntityFrameworkCore;

namespace AngularStandaloneDemo.Services
{
    public class PatientTaskService : IPatientTaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PatientTaskService> _logger;

        public PatientTaskService(ApplicationDbContext context, ILogger<PatientTaskService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<PatientTaskDto>> GetAllTasksAsync()
        {
            return await _context.PatientTasks
                .Include(t => t.Patient)
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .Select(t => MapToDto(t))
                .ToListAsync();
        }

        public async Task<PatientTaskDto?> GetTaskByIdAsync(int id)
        {
            var task = await _context.PatientTasks
                .Include(t => t.Patient)
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .FirstOrDefaultAsync(t => t.Id == id);

            return task != null ? MapToDto(task) : null;
        }

        public async Task<IEnumerable<PatientTaskDto>> GetTasksByPatientIdAsync(int patientId)
        {
            return await _context.PatientTasks
                .Include(t => t.Patient)
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .Where(t => t.PatientId == patientId)
                .Select(t => MapToDto(t))
                .ToListAsync();
        }

        public async Task<IEnumerable<PatientTaskDto>> GetTasksByAssignedUserIdAsync(int userId)
        {
            return await _context.PatientTasks
                .Include(t => t.Patient)
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .Where(t => t.AssignedToUserId == userId)
                .Select(t => MapToDto(t))
                .ToListAsync();
        }

        public async Task<IEnumerable<PatientTaskDto>> GetOverdueTasksAsync()
        {
            return await _context.PatientTasks
                .Include(t => t.Patient)
                .Include(t => t.AssignedToUser)
                .Include(t => t.CreatedByUser)
                .Where(t => t.DueDate < DateTime.UtcNow && t.Status != Enums.TaskStatus.Completed)
                .Select(t => MapToDto(t))
                .ToListAsync();
        }

        public async Task<PatientTaskDto> CreateTaskAsync(CreatePatientTaskDto createDto, int createdByUserId)
        {
            // Validate patient exists
            var patientExists = await _context.Patients.AnyAsync(p => p.Id == createDto.PatientId);
            if (!patientExists)
            {
                throw new InvalidOperationException($"Patient with ID {createDto.PatientId} not found");
            }

            // Validate assigned user exists
            var userExists = await _context.Users.AnyAsync(u => u.UserID == createDto.AssignedToUserId);
            if (!userExists)
            {
                throw new InvalidOperationException($"User with ID {createDto.AssignedToUserId} not found");
            }

            // Create the task entity
            var task = new PatientTask
            {
                PatientId = createDto.PatientId,
                Title = createDto.Title,
                Description = createDto.Description,
                Priority = createDto.Priority,
                Status = Enums.TaskStatus.NotStarted, // ALWAYS set to NotStarted for new tasks
                DueDate = createDto.DueDate,
                AssignedToUserId = createDto.AssignedToUserId,
                CreatedByUserId = createdByUserId, // SET FROM JWT TOKEN
                CreatedDate = DateTime.UtcNow,
                IsRecurring = createDto.IsRecurring,
                RecurringPattern = createDto.RecurringPattern
            };

            _context.PatientTasks.Add(task);
            await _context.SaveChangesAsync();

            // Reload with navigation properties
            await _context.Entry(task)
                .Reference(t => t.Patient)
                .LoadAsync();
            await _context.Entry(task)
                .Reference(t => t.AssignedToUser)
                .LoadAsync();
            await _context.Entry(task)
                .Reference(t => t.CreatedByUser)
                .LoadAsync();

            return MapToDto(task);
        }

        public async Task<PatientTaskDto?> UpdateTaskAsync(int id, UpdatePatientTaskDto updateDto, int modifiedByUserId)
        {
            var task = await _context.PatientTasks.FindAsync(id);
            if (task == null) return null;

            // Validate patient exists
            //var patientExists = await _context.Patients.AnyAsync(p => p.Id == updateDto.PatientId);
            //if (!patientExists)
            //{
            //    throw new InvalidOperationException($"Patient with ID {updateDto.PatientId} not found");
            //}

            // Validate assigned user exists
            var userExists = await _context.Users.AnyAsync(u => u.UserID == updateDto.AssignedToUserId);
            if (!userExists)
            {
                throw new InvalidOperationException($"User with ID {updateDto.AssignedToUserId} not found");
            }

            // Update fields
            //task.PatientId = updateDto.PatientId;
            task.Title = updateDto.Title;
            task.Description = updateDto.Description;
            task.Priority = updateDto.Priority;
            task.Status = updateDto.Status;
            task.DueDate = updateDto.DueDate;
            task.AssignedToUserId = updateDto.AssignedToUserId;
            task.IsRecurring = updateDto.IsRecurring;
            task.RecurringPattern = updateDto.RecurringPattern;
            task.LastModifiedDate = DateTime.UtcNow;

            // Set completed date if status changed to completed
            if (updateDto.Status == Enums.TaskStatus.Completed && task.CompletedDate == null)
            {
                task.CompletedDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            // Reload with navigation properties
            await _context.Entry(task)
                .Reference(t => t.Patient)
                .LoadAsync();
            await _context.Entry(task)
                .Reference(t => t.AssignedToUser)
                .LoadAsync();
            await _context.Entry(task)
                .Reference(t => t.CreatedByUser)
                .LoadAsync();

            return MapToDto(task);
        }

        public async Task<PatientTaskDto?> CompleteTaskAsync(int id)
        {
            var task = await _context.PatientTasks.FindAsync(id);
            if (task == null) return null;

            task.Status = Enums.TaskStatus.Completed;
            task.CompletedDate = DateTime.UtcNow;
            task.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Reload with navigation properties
            await _context.Entry(task)
                .Reference(t => t.Patient)
                .LoadAsync();
            await _context.Entry(task)
                .Reference(t => t.AssignedToUser)
                .LoadAsync();
            await _context.Entry(task)
                .Reference(t => t.CreatedByUser)
                .LoadAsync();

            return MapToDto(task);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.PatientTasks.FindAsync(id);
            if (task == null) return false;

            _context.PatientTasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        // Helper method to map entity to DTO
        private PatientTaskDto MapToDto(PatientTask task)
        {
            return new PatientTaskDto
            {
                Id = task.Id,
                PatientId = task.PatientId,
                Title = task.Title,
                Description = task.Description,
                Priority = task.Priority,
                Status = task.Status,
                DueDate = task.DueDate,
                AssignedToUserId = task.AssignedToUserId,
                AssignedToUserName = task.AssignedToUser != null
                    ? $"{task.AssignedToUser.FirstName} {task.AssignedToUser.LastName}"
                    : null,
                CreatedByUserId = task.CreatedByUserId,
                CreatedByUserName = task.CreatedByUser != null
                    ? $"{task.CreatedByUser.FirstName} {task.CreatedByUser.LastName}"
                    : null,
                CreatedDate = task.CreatedDate,
                LastModifiedDate = task.LastModifiedDate,
                CompletedDate = task.CompletedDate,
                IsRecurring = task.IsRecurring,
                RecurringPattern = task.RecurringPattern,
                PatientName = task.Patient != null
                    ? $"{task.Patient.FirstName} {task.Patient.LastName}"
                    : null
            };
        }
    }
}
