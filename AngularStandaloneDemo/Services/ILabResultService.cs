using AngularStandaloneDemo.Models;

namespace AngularStandaloneDemo.Services
{
    public interface ILabResultService 
    {
        Task<List<LabResult>> GetLabResultsByPatientIdAsync(int patientId);
        Task<LabResult> GetLabResultByIdAsync(int patientId, int labId);
        Task<LabResult> CreateLabResultAsync(LabResult labResult);
        Task UpdateLabResultAsync(LabResult labResult);
        Task DeleteLabResultAsync(int patientId, int labId);
    }
}
