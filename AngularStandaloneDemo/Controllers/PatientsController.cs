using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularStandaloneDemo.Models;
using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Dtos;
using DoctorAppointmentSystem.DTOs;

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
            return await _context.Patients.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.PatientDetails)
                .Include(pd => pd!.WaitingLists)
                .Include(pd => pd!.Appointments)
                  .ThenInclude(a => a.Provider)
                .Include(pd => pd!.MedicalRecords)
                   .ThenInclude(mr => mr.Visits)
                .Include(pd => pd!.MedicalRecords)
                       .ThenInclude(mr => mr.Visits)
                         .ThenInclude(v => v.Medication)
                .Include(pd => pd!.MedicalRecords)
                    .ThenInclude(mr => mr.Allergies)
                .Include(pd => pd!.MedicalRecords)
                        .ThenInclude(mr => mr.LabResults)
                 .Include(pd => pd!.MedicalRecords)
                        .ThenInclude(mr => mr.Pressure)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }
          

            return patient;
        }

        // PUT: api/Patients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(patient).State = EntityState.Modified;

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

        [HttpPost]
        public async Task<ActionResult<Patient>> CreatePatient(PatientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patient = new Patient
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                GenderID = dto.GenderID,
                ContactNumber = dto.ContactNumber,
                Email = dto.Email,
                Address = dto.Address,
                EmergencyContactName = dto.EmergencyContactName,
                EmergencyContactNumber = dto.EmergencyContactNumber,
                InsuranceProvider = dto.InsuranceProvider,
                InsuranceNumber = dto.InsuranceNumber,
                NursID = dto.NursID,
                NursName = dto.NursName,
                PatientDoctorID = dto.PatientDoctorID,
                PatientDoctorName = dto.PatientDoctorName,
                RegistrationDate = DateTime.Now, // Always set by the server
                LastVisitDate = dto.LastVisitDate ?? DateTime.Now,
                
        };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
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
            //patient.LastVisitDate = visitDate;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/patients/{id}/info
        [HttpPut("{id}/info")]
        public async Task<IActionResult> UpdatePatientInfo(int id, [FromBody] PatientBasicInfoUpdate patientInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _context.UpdatePatientInfoAsync(id, patientInfo);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }


        // PATCH: api/patients/{id}/info/contact
        [HttpPatch("{id}/info/contact")]
        public async Task<IActionResult> UpdateContactInfo(int id, [FromBody] ContactInfoUpdate contactInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _context.UpdateContactInfoAsync(id, contactInfo);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // PATCH: api/patients/{id}/info/insurance
        [HttpPatch("{id}/info/insurance")]
        public async Task<IActionResult> UpdateInsuranceInfo(int id, [FromBody] InsuranceInfoUpdate insuranceInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _context.UpdateInsuranceInfoAsync(id, insuranceInfo);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // GET: api/Patients/search?term=smith
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Patient>>> SearchPatients(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return BadRequest("Search term cannot be empty");
            }

            var patients = await _context.Patients
                .Where(p => p.FirstName.Contains(term) ||
                            p.LastName.Contains(term) ||
                            p.InsuranceNumber.Contains(term))
                .ToListAsync();

            return patients;
        }
        // GET: api/Patients/appointments/5
        [HttpGet("appointments/{id}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetPatientAppointments(int id)
        {
            var appointments = await _context.Appointments
                .Where(a => a.PatientId == id)
                .ToListAsync();

            if (appointments == null || !appointments.Any())
            {
                return NotFound("No appointments found for this patient");
            }

            return appointments;
        }

        // GET: api/Patients/medicalrecords/5
        [HttpGet("medicalrecords/{id}")]
        public async Task<ActionResult<IEnumerable<MedicalRecord>>> GetPatientMedicalRecords(int id)
        {
            var records = await _context.MedicalRecords
                .Where(r => r.PatientId == id)
                .ToListAsync();

            if (records == null || !records.Any())
            {
                return NotFound("No medical records found for this patient");
            }

            return records;
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
