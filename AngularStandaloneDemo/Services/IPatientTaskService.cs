using AngularStandaloneDemo.Dtos;

namespace AngularStandaloneDemo.Services
{
    public interface IPatientTaskService
    {
        Task<IEnumerable<PatientTaskDto>> GetAllTasksAsync();
        Task<IEnumerable<PatientTaskDto>> GetTasksByPatientIdAsync(int patientId);
        Task<IEnumerable<PatientTaskDto>> GetTasksByAssignedUserIdAsync(int userId);
        Task<IEnumerable<PatientTaskDto>> GetOverdueTasksAsync();
        Task<PatientTaskDto?> GetTaskByIdAsync(int id);
        Task<PatientTaskDto> CreateTaskAsync(CreatePatientTaskDto createDto, int createdByUserId);
        Task<PatientTaskDto?> UpdateTaskAsync(int id, UpdatePatientTaskDto updateDto, int modifiedByUserId);
        Task<bool> DeleteTaskAsync(int id);
        Task<PatientTaskDto?> CompleteTaskAsync(int id);
    }
}
