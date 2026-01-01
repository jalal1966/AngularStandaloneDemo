using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace AngularStandaloneDemo.Services
{
    public class LabResultService : ILabResultService  // <-- ADD THIS INTERFACE
    {
        private readonly ApplicationDbContext _context;

        public LabResultService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Rest of your code stays exactly the same...
        public async Task<List<LabResult>> GetLabResultsByPatientIdAsync(int patientId)
        {
            return await _context.LabResults
                .Where(lr => lr.PatientId == patientId)
                .OrderByDescending(lr => lr.TestDate)
                .ToListAsync();
        }

        public async Task<LabResult> GetLabResultByIdAsync(int patientId, int labId)
        {
            return await _context.LabResults
                .FirstOrDefaultAsync(lr => lr.Id == labId && lr.PatientId == patientId);
        }

        public async Task<LabResult> CreateLabResultAsync(LabResult labResult)
        {
            _context.LabResults.Add(labResult);
            await _context.SaveChangesAsync();
            return labResult;
        }

        public async Task UpdateLabResultAsync(LabResult labResult)
        {
            _context.LabResults.Update(labResult);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLabResultAsync(int patientId, int labId)
        {
            var labResult = await _context.LabResults
                .FirstOrDefaultAsync(lr => lr.Id == labId && lr.PatientId == patientId);

            if (labResult != null)
            {
                _context.LabResults.Remove(labResult);
                await _context.SaveChangesAsync();
            }
        }
    }
}