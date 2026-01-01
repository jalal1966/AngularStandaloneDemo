using AngularStandaloneDemo.Models;

namespace AngularStandaloneDemo.Services
{
    public interface IPatientService
    {
        Task<Patient> GetPatientByIdAsync(int patientId);
    }
}
