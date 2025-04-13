using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularStandaloneDemo.Models;
using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Dtos;

namespace AngularStandaloneDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly Data.ApplicationDbContext _context;

        public PatientsController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Patients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            return await _context.Patients
                .Include(p => p.PatientDetails)
                .ToListAsync();
        }

        // GET: api/Patients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await _context.Patients
         .Include(p => p.PatientDetails)
             .ThenInclude(pd => pd.MedicalRecords)
                 .ThenInclude(mr => mr.Visits)
                     .ThenInclude(v => v.Medication)
         .Include(p => p.PatientDetails)
             .ThenInclude(pd => pd.MedicalRecords)
                 .ThenInclude(mr => mr.Allergies)
         .Include(p => p.PatientDetails)
             .ThenInclude(pd => pd.MedicalRecords)
                 .ThenInclude(mr => mr.LabResults)
         .Include(p => p.PatientDetails)
             .ThenInclude(pd => pd.Appointments)
         .Include(p => p.PatientDetails)
             .ThenInclude(pd => pd.MedicalRecords)
                 .ThenInclude(mr => mr.Immunizations)
         .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            return patient;
        }

        // PUT: api/Patients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }

            _context.Entry(patient).State = EntityState.Modified;
            _context.Entry(patient.PatientDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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

        // POST: api/Patients
        /* [HttpPost]
         public async Task<ActionResult<PatientDto>> PostPatient(PatientDto patient)
         {
             _context.Patients.Add(patient);
             await _context.SaveChangesAsync();

             return CreatedAtAction("GetPatient", new { id = patient.Id }, patient);
         }*/
        [HttpPost]
        public async Task<ActionResult<PatientDto>> PostPatient(PatientDto patientDto)
        {
            // Map DTO to model
            var patient = new Patient
            {
                FirstName = patientDto.FirstName,
                LastName = patientDto.LastName,
                DateOfBirth = patientDto.DateOfBirth,
                GenderID = patientDto.GenderID,
                ContactNumber = patientDto.ContactNumber,
                Email = patientDto.Email,
                Address = patientDto.Address,
                EmergencyContactName = patientDto.EmergencyContactName,
                EmergencyContactNumber = patientDto.EmergencyContactNumber,
                InsuranceProvider = patientDto.InsuranceProvider,
                InsuranceNumber = patientDto.InsuranceNumber,
                NursID = patientDto.NursID,
                NursName = patientDto.NursName,
                PatientDoctorName = patientDto.PatientDoctorName,
                PatientDoctorID = patientDto.PatientDoctorID,
                RegistrationDate = DateTime.Now, // Or patientDto.RegistrationDate if you want to use that value
                LastVisitDate = patientDto.LastVisitDate
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            // Update the DTO with the generated ID
            patientDto.Id = patient.Id;

            return CreatedAtAction("GetPatient", new { id = patient.Id }, patientDto);
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.PatientDetails)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Patients/5/UpdateLastVisit
        [HttpPut("{id}/UpdateLastVisit")]
        public async Task<IActionResult> UpdateLastVisit(int id, [FromBody] DateTime visitDate)
        {
            var patient = await _context.Patients
                .Include(p => p.PatientDetails)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            patient.LastVisitDate = visitDate;
            patient.PatientDetails.LastVisitDate = visitDate;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Patients/Search?query=smith
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<Patient>>> SearchPatients([FromQuery] string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return await GetPatients();
            }

            return await _context.Patients
                .Include(p => p.PatientDetails)
                .Where(p => p.FirstName.Contains(query) ||
                            p.LastName.Contains(query) ||
                            p.ContactNumber.Contains(query) ||
                            p.Email.Contains(query))
                .ToListAsync();
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}