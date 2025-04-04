using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Dtos;
using AngularStandaloneDemo.Enums;

namespace AngularStandaloneDemo.Controllers
{
    [ApiController]
    [Route("api/patients/{id}/info")]
    public class PatientInfoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatientInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/patients/5/info
        [HttpGet]
        public async Task<ActionResult<PatientInfoDto>> GetPatientInfo(int id)
        {
            var patient = await _context.Patients
           
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            var patientInfo = new PatientInfoDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                GenderID = patient.GenderID,
                Gender = (Gender)patient.GenderID, // Cast the integer to the enum
                //GenderName = patient.Gender?.Name,
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
                PatientDoctorID = patient.PatientDoctorID,
                RegistrationDate = patient.RegistrationDate,
                LastVisitDate = patient.LastVisitDate
            };

            return Ok(patientInfo);
        }

        // PUT: api/patients/5/info
        [HttpPut]
        public async Task<IActionResult> UpdatePatientInfo(int id, PatientInfoDto patientInfo)
        {
            if (id != patientInfo.Id)
            {
                return BadRequest();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            // Update patient basic info fields
            patient.FirstName = patientInfo.FirstName;
            patient.LastName = patientInfo.LastName;
            patient.DateOfBirth = patientInfo.DateOfBirth;
            patient.GenderID = patientInfo.GenderID;

            patient.ContactNumber = patientInfo.ContactNumber;
            patient.Email = patientInfo.Email;
            patient.Address = patientInfo.Address;
            patient.EmergencyContactName = patientInfo.EmergencyContactName;
            patient.EmergencyContactNumber = patientInfo.EmergencyContactNumber;
            patient.InsuranceProvider = patientInfo.InsuranceProvider;
            patient.InsuranceNumber = patientInfo.InsuranceNumber;
            patient.NursID = patientInfo.NursID;
            patient.NursName = patientInfo.NursName;
            patient.PatientDoctorName = patientInfo.PatientDoctorName;
            patient.PatientDoctorID = patientInfo.PatientDoctorID;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/patients/5/info/contact
        [HttpPatch("contact")]
        public async Task<IActionResult> UpdateContactInfo(int id, ContactInfoUpdateDto contactInfo)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            // Update only contact information
            patient.ContactNumber = contactInfo.ContactNumber;
            patient.Email = contactInfo.Email;
            patient.Address = contactInfo.Address;
            patient.EmergencyContactName = contactInfo.EmergencyContactName;
            patient.EmergencyContactNumber = contactInfo.EmergencyContactNumber;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/patients/5/info/insurance
        [HttpPatch("insurance")]
        public async Task<IActionResult> UpdateInsuranceInfo(int id, InsuranceUpdateDto insuranceInfo)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            // Update only insurance information
            patient.InsuranceProvider = insuranceInfo.InsuranceProvider;
            patient.InsuranceNumber = insuranceInfo.InsuranceNumber;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
