using AngularStandaloneDemo.Models;
using AngularStandaloneDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace AngularStandaloneDemo.Controllers
{
    [Route("api/patients/{patientId}/lab-results")]
    [ApiController]
    public class LabResultsController : ControllerBase
    {
        private readonly ILabResultService _labResultService;
        private readonly IEmailService _emailService;
        private readonly ILogger<LabResultsController> _logger;
        private readonly Data.ApplicationDbContext _context;

        public LabResultsController(
            ILabResultService labResultService,
            IEmailService emailService,
            ILogger<LabResultsController> logger,
            Data.ApplicationDbContext context)
        {
            _labResultService = labResultService;
            _emailService = emailService;
            _logger = logger;
            _context = context;
        }

        // GET: api/patients/{patientId}/lab-results
        [HttpGet]
        public async Task<ActionResult<List<LabResult>>> GetLabResults(int patientId)
        {
            try
            {
                var labResults = await _labResultService.GetLabResultsByPatientIdAsync(patientId);
                return Ok(labResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving lab results for patient {patientId}");
                return StatusCode(500, new { message = "Error retrieving lab results" });
            }
        }

        // GET: api/patients/{patientId}/lab-results/{labId}
        [HttpGet("{labId}")]
        public async Task<ActionResult<LabResult>> GetLabResult(int patientId, int labId)
        {
            try
            {
                var labResult = await _labResultService.GetLabResultByIdAsync(patientId, labId);

                if (labResult == null)
                {
                    return NotFound(new { message = "Lab result not found" });
                }

                return Ok(labResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving lab result {labId} for patient {patientId}");
                return StatusCode(500, new { message = "Error retrieving lab result" });
            }
        }

        // POST: api/patients/{patientId}/lab-results
        [HttpPost]
        public async Task<ActionResult<LabResult>> CreateLabResult(int patientId, [FromBody] LabResult labResult)
        {
            try
            {
                labResult.PatientId = patientId;
                var createdLabResult = await _labResultService.CreateLabResultAsync(labResult);
                return CreatedAtAction(nameof(GetLabResult),
                    new { patientId = patientId, labId = createdLabResult.Id },
                    createdLabResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating lab result for patient {patientId}");
                return StatusCode(500, new { message = "Error creating lab result" });
            }
        }

        // PUT: api/patients/{patientId}/lab-results/{labId}
        [HttpPut("{labId}")]
        public async Task<IActionResult> UpdateLabResult(int patientId, int labId, [FromBody] LabResult labResult)
        {
            try
            {
                if (labId != labResult.Id)
                {
                    return BadRequest(new { message = "Lab result ID mismatch" });
                }

                labResult.PatientId = patientId;
                await _labResultService.UpdateLabResultAsync(labResult);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating lab result {labId} for patient {patientId}");
                return StatusCode(500, new { message = "Error updating lab result" });
            }
        }

        // DELETE: api/patients/{patientId}/lab-results/{labId}
        [HttpDelete("{labId}")]
        public async Task<IActionResult> DeleteLabResult(int patientId, int labId)
        {
            try
            {
                await _labResultService.DeleteLabResultAsync(patientId, labId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting lab result {labId} for patient {patientId}");
                return StatusCode(500, new { message = "Error deleting lab result" });
            }
        }

        // POST: api/patients/{patientId}/lab-results/{labId}/email
        [HttpPost("{labId}/email")]
        public async Task<IActionResult> EmailLabResult(
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
    }

    // DTO for email request
    public class EmailRequest
    {
        public string Email { get; set; }
    }
}