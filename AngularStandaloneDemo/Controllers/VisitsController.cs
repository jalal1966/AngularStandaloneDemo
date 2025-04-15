using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularStandaloneDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitsController : ControllerBase
    {
        private readonly Data.ApplicationDbContext _context;

        public VisitsController(Data.ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: api/Visits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Visit>>> GetVisits() => await _context.Visits
            .Include(v => v.Diagnosis)
            .Include(v => v.Medication) // Added this to include medications
            .ToListAsync();
               

        // GET: api/Visits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Visit>> GetVisit(int id)
        {
            var visit = await _context.Visits
                //.Include(v => v.PatientId)
                .Include(v => v.Diagnosis)
                .Include(v => v.Medication)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (visit == null)
            {
                return NotFound();
            }

            return visit;
        }

        // GET: api/Visits/Patient/5
        [HttpGet("Patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<Visit>>> GetVisitsByPatient(int patientId)
        {
            return await _context.Visits
                .Where(v => v.PatientId == patientId)
                .Include(v => v.Diagnosis)
                .Include(m => m.Medication)
                .ToListAsync();
        }

        public class VisitWrapper
        {
            public Visit Visit { get; set; } = null!;
        }

        [HttpPost]
        public async Task<ActionResult<Visit>> CreateVisit([FromBody] VisitWrapper wrapper)
        {
            if (wrapper == null || wrapper.Visit == null)
            {
                return BadRequest("Visit data is required");
            }

            var visit = wrapper.Visit;

            // Create a new instance to avoid any potential tracking issues
            var newVisit = new Visit
            {
                PatientId = visit.PatientId,
                VisitDate = visit.VisitDate,
                ProviderName = visit.ProviderName,
                ProviderId = visit.ProviderId,
                VisitType = visit.VisitType,
                Reason = visit.Reason,
                Assessment = visit.Assessment,
                PlanTreatment = visit.PlanTreatment,
                Notes = visit.Notes,
                FollowUpRequired = visit.FollowUpRequired,
                FollowUpInstructions = visit.FollowUpInstructions,
                FollowUpDate = visit.FollowUpDate,
                FollowUpReason = visit.FollowUpReason,
                FollowUpProviderName = visit.FollowUpProviderName,
                FollowUpProviderId = visit.FollowUpProviderId,
                FollowUpType = visit.FollowUpType,
                MedicalRecordId = visit.MedicalRecordId
            };

            // Add diagnoses
            foreach (var diagnosis in visit.Diagnosis)
            {
                newVisit.Diagnosis.Add(new Diagnosis
                {
                    DiagnosisCode = diagnosis.DiagnosisCode,
                    Description = diagnosis.Description,
                    DiagnosisDate = diagnosis.DiagnosisDate,
                    IsActive = diagnosis.IsActive
                });
            }

            // Add medications
            foreach (var medication in visit.Medication)
            {
                newVisit.Medication.Add(new Medication
                {
                    Name = medication.Name,
                    Dosage = medication.Dosage,
                    Frequency = medication.Frequency,
                    StartDate = medication.StartDate,
                    EndDate = medication.EndDate,
                    PrescribingProvider = medication.PrescribingProvider,
                    Purpose = medication.Purpose,
                    IsActive = medication.IsActive
                });
            }

            _context.Visits.Add(newVisit);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVisit), new { id = newVisit.Id }, newVisit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVisit(int id, Visit visit)
        {
            if (id != visit.Id)
            {
                return BadRequest();
            }

            // First get the existing visit with its related entities
            var existingVisit = await _context.Visits
                .Include(v => v.Diagnosis)
                .Include(v => v.Medication)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (existingVisit == null)
            {
                return NotFound();
            }

            try
            {
                // Update basic visit properties
                _context.Entry(existingVisit).CurrentValues.SetValues(visit);

                // Handle diagnoses
                // Remove diagnoses that are not in the updated list
                foreach (var existingDiagnosis in existingVisit.Diagnosis.ToList())
                {
                    if (!visit.Diagnosis.Any(d => d.Id == existingDiagnosis.Id))
                    {
                        _context.Diagnosis.Remove(existingDiagnosis);
                    }
                }

                // Update existing diagnoses and add new ones
                foreach (var diagnosisModel in visit.Diagnosis)
                {
                    var existingDiagnosis = existingVisit.Diagnosis
                        .FirstOrDefault(d => d.Id == diagnosisModel.Id && diagnosisModel.Id != 0);

                    if (existingDiagnosis != null)
                    {
                        // Update existing diagnosis - explicitly update all properties
                        existingDiagnosis.DiagnosisCode = diagnosisModel.DiagnosisCode;
                        existingDiagnosis.Description = diagnosisModel.Description;
                        existingDiagnosis.DiagnosisDate = diagnosisModel.DiagnosisDate;
                        existingDiagnosis.IsActive = diagnosisModel.IsActive;
                        // Add any other properties that need to be updated
                    }
                    else
                    {
                        // Add new diagnosis
                        existingVisit.Diagnosis.Add(diagnosisModel);
                    }
                }

                // Handle medications - same approach as diagnoses
                foreach (var existingMedication in existingVisit.Medication.ToList())
                {
                    if (!visit.Medication.Any(m => m.Id == existingMedication.Id))
                    {
                        _context.Medications.Remove(existingMedication);
                    }
                }

                foreach (var medicationModel in visit.Medication)
                {
                    var existingMedication = existingVisit.Medication
                        .FirstOrDefault(m => m.Id == medicationModel.Id && medicationModel.Id != 0);

                    if (existingMedication != null)
                    {
                        // Update existing medication - explicitly update all properties
                        existingMedication.Name = medicationModel.Name;
                        existingMedication.Dosage = medicationModel.Dosage;
                        existingMedication.Frequency = medicationModel.Frequency;
                        existingMedication.StartDate = medicationModel.StartDate;
                        existingMedication.EndDate = medicationModel.EndDate;
                        existingMedication.PrescribingProvider = medicationModel.PrescribingProvider;
                        existingMedication.Purpose = medicationModel.Purpose;
                        // Add any other properties that need to be updated
                    }
                    else
                    {
                        // Add new medication
                        existingVisit.Medication.Add(medicationModel);
                    }
                }

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisitExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE: api/Visits/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisit(int id)
        {
            var visit = await _context.Visits.FindAsync(id);
            if (visit == null)
            {
                return NotFound();
            }

            _context.Visits.Remove(visit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VisitExists(int id)
        {
            return _context.Visits.Any(e => e.Id == id);
        }
    }
}