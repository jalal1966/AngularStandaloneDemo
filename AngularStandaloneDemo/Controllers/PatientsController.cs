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

namespace AngularStandaloneDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public PatientsController(ApplicationDbContext context, IConfiguration configuration, IAuthService authService)
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
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
                return NotFound();

            return Ok(new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                GenderID = patient.GenderID,
                ContactNumber = patient.ContactNumber,
                Email = patient.Email,
                Address = patient.Address,
                EmergencyContactName = patient.EmergencyContactName,
                EmergencyContactNumber = patient.EmergencyContactNumber,
                InsuranceProvider = patient.InsuranceProvider,
                InsuranceNumber = patient.InsuranceNumber,   
                NursID = patient.NursID,
                NursName = patient.NursName,
                PatientDoctorName = patient.PatientDoctorName,
                PatientDoctorID = patient.PatientDoctorID
            });
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
    }

    [Route("api/[controller]")]
    [ApiController]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MedicalRecordsController(ApplicationDbContext context) => _context = context;

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<MedicalRecordDto>>> GetMedicalRecords(int patientId)
        {
            // Verify the patient exists
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Id == patientId);

            if (patient == null)
                return NotFound("Patient not found");

            var records = await _context.MedicalRecords
                .Where(m => m.PatientId == patientId)
                .Select(m => new MedicalRecordDto
                {
                    Id = m.Id,
                    RecordDate = m.RecordDate,
                    Diagnosis = m.Diagnosis,
                    Treatment = m.Treatment,
                    Medications = m.Medications,
                    Notes = m.Notes,
                    IsFollowUpRequired = m.IsFollowUpRequired,
                    FollowUpDate = m.FollowUpDate,
                    PatientId = m.PatientId,

                })
                .ToListAsync();

            return Ok(records);
        }

        // Implement other CRUD endpoints for medical records
    }
}