using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace AngularStandaloneDemo.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _context;

        public PatientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Patient> GetPatientByIdAsync(int patientId)
        {
            return await _context.Patients.FindAsync(patientId);
        }
    }
}
