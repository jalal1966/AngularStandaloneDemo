using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Models;
using AngularStandaloneDemo.Dtos;
using System.Security.Claims;
using AngularStandaloneDemo.Services;
using System.Configuration;
using AngularStandaloneDemo.Enums;

namespace AngularStandaloneDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly Data.ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public PatientsController(Data.ApplicationDbContext context, IConfiguration configuration, IAuthService authService)
        {
            _context = context;
            _configuration = configuration;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients()
        {
            // Since we no longer require authorization, we'll get all patients
            var patients = await _context.Patients
            .Select(p => new PatientDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                DateOfBirth = p.DateOfBirth,
                GenderID = p.GenderID,
                Gender = (Gender)p.GenderID,
                ContactNumber = p.ContactNumber,
                Email = p.Email,
                Address = p.Address,
                EmergencyContactName = p.EmergencyContactName,
                EmergencyContactNumber = p.EmergencyContactNumber,
                InsuranceProvider = p.InsuranceProvider,
                InsuranceNumber = p.InsuranceNumber,
                NursID = p.NursID,
                NursName = p.NursName,
                PatientDoctorName = p.PatientDoctorName,
                PatientDoctorID = p.PatientDoctorID,
                RegistrationDate = p.RegistrationDate,
                LastVisitDate = p.LastVisitDate,
            })
            .ToListAsync();

            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetPatient(int id)
        {
            var patient = await _context.Patients.AsNoTracking()
                .Select(p => new {
                    Patient = p,
                    MedicalRecord = p.MedicalRecords.FirstOrDefault(),
                    p.Allergies,
                    Medications = p.Medications.Where(m => m.IsActive),
                    Visits = p.Visits.OrderByDescending(v => v.VisitDate).Take(5),
                    LabResults = p.LabResults.OrderByDescending(l => l.TestDate).Take(10),
                   Immunizations = p.Immunizations.OrderByDescending(i => i.Id).Take(10)
                })
                .FirstOrDefaultAsync(p => p.Patient.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            // var medicalRecord = patient.MedicalRecord.FirstOrDefault();

            var patientDetail = new PatientDetailDto
            {
                Id = patient.Patient.Id,
                FirstName = patient.Patient.FirstName,
                LastName = patient.Patient.LastName,
                DateOfBirth = patient.Patient.DateOfBirth,
                GenderID = patient.Patient.GenderID,
                ContactNumber = patient.Patient.ContactNumber,
                Email = patient.Patient.Email,
                Address = patient.Patient.Address,
                EmergencyContactName = patient.Patient.EmergencyContactName,
                EmergencyContactNumber = patient.Patient.EmergencyContactNumber,
                InsuranceProvider = patient.Patient.InsuranceProvider,
                InsuranceNumber = patient.Patient.InsuranceNumber,
                NursID = patient.Patient.NursID,
                NursName = patient.Patient.NursName,
                PatientDoctorName = patient.Patient.PatientDoctorName,
                PatientDoctorID = patient.Patient.PatientDoctorID,
                RegistrationDate = patient.Patient.RegistrationDate,
                LastVisitDate = patient.Patient.LastVisitDate,

                // Medical record data
                Height = patient.MedicalRecord?.Height,
                Weight = patient.MedicalRecord?.Weight,
                BMI = patient.MedicalRecord?.Bmi,
                BloodType = patient.MedicalRecord?.BloodType,

                // Related data
                Allergies = patient.Allergies.Select(a => new AllergyDto
                {
                    Id = a.Id,
                    AllergyType = a.AllergyType,
                    Name = a.Name,
                    Reaction = a.Reaction,
                    Severity = a.Severity
                }).ToList(),

                CurrentMedications = patient.Medications.Select(m => new MedicationDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Dosage = m.Dosage,
                    Frequency = m.Frequency,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate,
                    PrescribingProvider = m.PrescribingProvider,
                    Purpose = m.Purpose
                }).ToList(),

                RecentVisits = patient.Visits.Select(v => new VisitSummaryDto
                {
                    Id = v.Id,
                    VisitDate = v.VisitDate,
                    ProviderName = v.ProviderName,
                    VisitType = v.VisitType,
                    Reason = v.Reason
                }).ToList(),

                RecentLabResults = patient.LabResults.Select(l => new LabResultDto
                {
                    Id = l.Id,
                    TestDate = l.TestDate,
                    TestName = l.TestName,
                    Result = l.Result,
                    ReferenceRange = l.ReferenceRange
                }).ToList(),

                Immunizations = patient.Immunizations.Select(i => new ImmunizationDto
                 {
                    Id = i.Id,
                    VaccineName = i.VaccineName,
                    AdministrationDate = i.AdministrationDate,  // Changed from DateAdministered to AdministrationDate
                    AdministeringProvider = i.AdministeringProvider, // Changed from AdministeredBy to AdministeringProvider
                    LotNumber = i.LotNumber,
                    NextDoseDate = i.NextDoseDate,
                    // Manufacturer property is missing in your Immunization class
                    Manufacturer = i.Manufacturer // Setting this to null since it doesn't exist in the model
                }).ToList()
            };

            return Ok(patientDetail);
        }

        [HttpPost]
        public async Task<ActionResult<PatientDto>> CreatePatient(PatientDto patientDto)
        {
            // Since we don't have user authentication, we'll need to handle the UserID differently
            // For now, we'll assign a default or null UserID, depending on your DB constraints

#pragma warning disable IDE0090 // Use 'new(...)'
            Patient patient = new Patient
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
                RegistrationDate = DateTime.UtcNow,
                NursID = patientDto.NursID,
                NursName = patientDto.NursName,
                PatientDoctorName = patientDto.PatientDoctorName,
                PatientDoctorID = patientDto.PatientDoctorID,
                MedicalRecords = new List<MedicalRecord>() // Initialize MedicalRecords list
            };
#pragma warning restore IDE0090 // Use 'new(...)'

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            patientDto.Id = patient.Id;

            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patientDto);
        }

        // Add Update and Delete endpoints
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

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Patients/Search?query=Smith
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<PatientDto>>> SearchPatients(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return await GetPatients();
            }

            var patients = await _context.Patients
                .Where(p => p.FirstName.Contains(query) ||
                            p.LastName.Contains(query) ||
                            p.Email.Contains(query) ||
                            p.ContactNumber.Contains(query))
                .Select(p => new PatientDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    DateOfBirth = p.DateOfBirth,
                    GenderID = p.GenderID,
                    Gender = (Gender)p.GenderID,
                    ContactNumber = p.ContactNumber,
                    Email = p.Email,
                    Address = p.Address,
                    EmergencyContactName = p.EmergencyContactName,
                    EmergencyContactNumber = p.EmergencyContactNumber,
                    InsuranceProvider = p.InsuranceProvider,
                    InsuranceNumber = p.InsuranceNumber,
                    NursID = p.NursID,
                    NursName = p.NursName,
                    PatientDoctorName = p.PatientDoctorName,
                    PatientDoctorID = p.PatientDoctorID,
                    RegistrationDate = p.RegistrationDate,
                    LastVisitDate = p.LastVisitDate
                })
                .ToListAsync();

            return Ok(patients);
        }

        // GET: api/Patients/GetByDoctor/5
        [HttpGet("GetByDoctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatientsByDoctor(int doctorId)
        {
            var patients = await _context.Patients
                .Where(p => p.PatientDoctorID == doctorId)
                .Select(p => new PatientDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    DateOfBirth = p.DateOfBirth,
                    GenderID = p.GenderID,
                    Gender = (Gender)p.GenderID,
                    ContactNumber = p.ContactNumber,
                    Email = p.Email,
                    Address = p.Address,
                    EmergencyContactName = p.EmergencyContactName,
                    EmergencyContactNumber = p.EmergencyContactNumber,
                    InsuranceProvider = p.InsuranceProvider,
                    InsuranceNumber = p.InsuranceNumber,
                    NursID = p.NursID,
                    NursName = p.NursName,
                    PatientDoctorName = p.PatientDoctorName,
                    PatientDoctorID = p.PatientDoctorID,
                    RegistrationDate = p.RegistrationDate,
                    LastVisitDate = p.LastVisitDate
                })
                .ToListAsync();

            return Ok(patients);
        }

        // PUT: api/Patients/5/UpdateLastVisit
        [HttpPut("{id}/UpdateLastVisit")]
        public async Task<IActionResult> UpdateLastVisit(int id, [FromBody] DateTime visitDate)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            patient.UpdateLastVisit(visitDate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}