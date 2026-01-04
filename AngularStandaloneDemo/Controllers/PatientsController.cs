using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Dtos;
using AngularStandaloneDemo.Models;
using AngularStandaloneDemo.Services;
using DoctorAppointmentSystem.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularStandaloneDemo.Controllers
{
   
    
    [Route("api/[controller]")]
   
    public class PatientsController : ControllerBase
    {
        private readonly ILabResultService _labResultService;
        private readonly IEmailService _emailService;
        private readonly ILogger<PatientsController> _logger;
        private readonly Data.ApplicationDbContext _context;

        public PatientsController(Data.ApplicationDbContext context, ILabResultService labResultService,
            IEmailService emailService,
            ILogger<PatientsController> logger)
        {
            _context = context;
            _labResultService = labResultService;
            _emailService = emailService;
            _logger = logger;
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

        // POST: api/Patients/{patientId}/lab-results/{labId}/email
        [HttpPost("{patientId}/lab-results/{labId}/email")]
        public async Task<IActionResult> EmailLabResults(
            int patientId,
            int labId,
            [FromBody] EmailRequest request)
        {
            try
            {
                _logger.LogInformation($"Attempting to email lab results. PatientId: {patientId}, LabId: {labId}, Email: {request.Email}");

                // Validate input
                if (string.IsNullOrEmpty(request.Email))
                {
                    _logger.LogWarning("Email address is missing");
                    return BadRequest(new { message = "Email address is required" });
                }

                // Get the patient
                var patient = await _context.Patients.FindAsync(patientId);
                if (patient == null)
                {
                    _logger.LogWarning($"Patient not found. PatientId: {patientId}");
                    return NotFound(new { message = "Patient not found" });
                }

                // Get the lab result
                var labResult = await _labResultService.GetLabResultByIdAsync(patientId, labId);

                if (labResult == null)
                {
                    _logger.LogWarning($"Lab result not found. PatientId: {patientId}, LabId: {labId}");
                    return NotFound(new { message = "Lab result not found" });
                }

                // Create email content
                string subject = "Your Lab Test Results";
                string body = $@"
                    <html>
                    <head>
                        <style>
                            body {{ font-family: Arial, sans-serif; color: #333; }}
                            .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                            .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
                            .content {{ background-color: #f9f9f9; padding: 20px; border: 1px solid #ddd; border-radius: 0 0 5px 5px; }}
                            table {{ border-collapse: collapse; width: 100%; margin: 20px 0; }}
                            td {{ padding: 12px; border: 1px solid #ddd; }}
                            .label {{ font-weight: bold; background-color: #f0f0f0; width: 40%; }}
                            .footer {{ margin-top: 20px; padding-top: 20px; border-top: 1px solid #ddd; font-size: 12px; color: #666; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h2>Lab Test Results</h2>
                            </div>
                            <div class='content'>
                                <p>Dear {patient.FirstName} {patient.LastName},</p>
                                <p>Your lab test results are ready for review:</p>
                                
                                <table>
                                    <tr>
                                        <td class='label'>Test Name:</td>
                                        <td>{labResult.TestName}</td>
                                    </tr>
                                    <tr>
                                        <td class='label'>Test Date:</td>
                                        <td>{labResult.TestDate:MMMM dd, yyyy}</td>
                                    </tr>
                                    <tr>
                                        <td class='label'>Result:</td>
                                        <td><strong>{labResult.Result}</strong></td>
                                    </tr>
                                    <tr>
                                        <td class='label'>Reference Range:</td>
                                        <td>{labResult.ReferenceRange ?? "N/A"}</td>
                                    </tr>
                                    <tr>
                                        <td class='label'>Ordering Provider:</td>
                                        <td>{labResult.OrderingProvider ?? "N/A"}</td>
                                    </tr>
                                </table>
                                
                                {(!string.IsNullOrEmpty(labResult.Notes) ? $"<div style='background-color: #fff3cd; padding: 15px; border-left: 4px solid #ffc107; margin: 20px 0;'><strong>Notes:</strong><br/>{labResult.Notes}</div>" : "")}
                                
                                <div class='footer'>
                                    <p>If you have any questions about these results, please contact your healthcare provider.</p>
                                    <p><strong>Important:</strong> This is an automated email. Please do not reply to this message.</p>
                                    <p>Best regards,<br/>Your Healthcare Team</p>
                                </div>
                            </div>
                        </div>
                    </body>
                    </html>";

                // Send email
                bool emailSent = await _emailService.SendEmailAsync(request.Email, subject, body);

                if (emailSent)
                {
                    _logger.LogInformation($"Email sent successfully to {request.Email}");
                    return Ok(new { message = $"Lab results successfully emailed to {request.Email}" });
                }
                else
                {
                    _logger.LogError("Failed to send email");
                    return StatusCode(500, new { message = "Failed to send email. Please check server logs and email configuration." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email for lab result {labId}: {ex.Message}");
                return StatusCode(500, new { message = $"An error occurred while sending email: {ex.Message}" });
            }
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }

   
}