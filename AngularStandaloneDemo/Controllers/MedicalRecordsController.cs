﻿using System;
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
        private readonly ApplicationDbContext _context;

        public MedicalRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/patients/5/medical-record
        [HttpGet]
        public async Task<ActionResult<MedicalRecord>> GetMedicalRecord(int patientId)
        {
            var medicalRecord = await _context.MedicalRecords
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
            existingRecord.BMI = medicalRecord.BMI;
            existingRecord.BloodType = medicalRecord.BloodType;
            existingRecord.ChronicConditions = medicalRecord.ChronicConditions;
            existingRecord.SurgicalHistory = medicalRecord.SurgicalHistory;
            existingRecord.FamilyMedicalHistory = medicalRecord.FamilyMedicalHistory;
            existingRecord.SocialHistory = medicalRecord.SocialHistory;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
