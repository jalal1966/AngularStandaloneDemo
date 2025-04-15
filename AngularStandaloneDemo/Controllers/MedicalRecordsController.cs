using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Models;

namespace AngularStandaloneDemo.Controllers
{
    [ApiController]
    [Route("api/patients/{patientId}/medical-record")]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly Data.ApplicationDbContext _context;

        public MedicalRecordsController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/patients/5/medical-record
        [HttpGet]
        public async Task<ActionResult<MedicalRecord>> GetMedicalRecord(int patientId)
        {
            var medicalRecord = await _context.MedicalRecords
             .Include(m => m.Visits)                          // Include visits
                .ThenInclude(v => v.Medication)             // Include medications for each visit
             .Include(m => m.Visits)
                .ThenInclude(v => v.Diagnosis)               // Include diagnosis for each visit
            .Include(m => m.Allergies)                       // Include allergies
            .Include(m => m.LabResults)                      // Include lab results
            .Include(m => m.Immunizations)                   // Include immunizations
            .FirstOrDefaultAsync(m => m.PatientId == patientId);

            if (medicalRecord == null)
            {
                return NotFound();
            }

            return Ok(medicalRecord);
        }

        // POST: api/patients/5/medical-record
        [HttpPost]
        public async Task<ActionResult<MedicalRecord>> CreateMedicalRecord(int patientId, MedicalRecord medicalRecord)
        {
            medicalRecord.PatientId = patientId;

            _context.MedicalRecords.Add(medicalRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMedicalRecord), new { patientId }, medicalRecord);
        }

        // PUT: api/patients/5/medical-record
        [HttpPut]
        public async Task<IActionResult> UpdateMedicalRecord(int patientId, MedicalRecord medicalRecord)
        {
            var existingRecord = await _context.MedicalRecords
                .FirstOrDefaultAsync(m => m.PatientId == patientId);

            if (existingRecord == null)
            {
                return NotFound();
            }

            // Update properties
            existingRecord.Height = medicalRecord.Height;
            existingRecord.Weight = medicalRecord.Weight;
            existingRecord.Bmi = medicalRecord.Bmi;
            existingRecord.BloodType = medicalRecord.BloodType;
            existingRecord.ChronicConditions = medicalRecord.ChronicConditions;
            existingRecord.SurgicalHistory = medicalRecord.SurgicalHistory;
            existingRecord.FamilyMedicalHistory = medicalRecord.FamilyMedicalHistory;
            existingRecord.SocialHistory = medicalRecord.SocialHistory;
            existingRecord.RecordDate = medicalRecord.RecordDate;
            existingRecord.Visits = medicalRecord.Visits;
            existingRecord.Notes = medicalRecord.Notes;
            existingRecord.IsFollowUpRequired = medicalRecord.IsFollowUpRequired;
            existingRecord.FollowUpDate = medicalRecord.FollowUpDate;  // Add this line
            existingRecord.UserID = medicalRecord.UserID;


            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
