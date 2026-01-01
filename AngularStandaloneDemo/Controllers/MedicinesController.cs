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
    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MedicinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Medicines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicine>>> GetMedicines()
        {
            return await _context.Medicines.ToListAsync();
        }

        // GET: api/Medicines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Medicine>> GetMedicine(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);

            if (medicine == null)
            {
                return NotFound();
            }

            return medicine;
        }

        // Add this method to the MedicinesController class
        // POST: api/Medicines/check-and-create
        [HttpPost("check-and-create")]
        public async Task<ActionResult<MedicineCheckResult>> CheckAndCreateMedicine(Medicine medicine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if a medicine with the same name, packaging, and company already exists
            var existingMedicine = await _context.Medicines
                .FirstOrDefaultAsync(m =>
                    m.Name.ToLower() == medicine.Name.ToLower() &&
                    m.Packaging.ToLower() == medicine.Packaging.ToLower() &&
                    m.Company.ToLower() == medicine.Company.ToLower());

            if (existingMedicine != null)
            {
                return Ok(new MedicineCheckResult
                {
                    Exists = true,
                    Message = "Medicine already exists in the table.",
                    ExistingMedicineId = existingMedicine.Id
                });
            }

            // If no existing medicine, add the new medicine
            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            return Ok(new MedicineCheckResult
            {
                Exists = false,
                Message = "Medicine was added successfully.",
                CreatedMedicineId = medicine.Id
            });
        }

       


        // POST: api/Medicines
        [HttpPost]
        public async Task<ActionResult<Medicine>> CreateMedicine(Medicine medicine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMedicine), new { id = medicine.Id }, medicine);
        }

        // PUT: api/Medicines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedicine(int id, Medicine medicine)
        {
            if (id != medicine.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(medicine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicineExists(id))
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

        // DELETE: api/Medicines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }

            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicineExists(int id)
        {
            return _context.Medicines.Any(e => e.Id == id);
        }

        // POST: api/Medicines/bulk-import
        [HttpPost("bulk-import")]
        public async Task<ActionResult<ImportResult>> BulkImport(MedicineImportRequest request)
        {
            if (request?.Medicines == null || !request.Medicines.Any())
            {
                return BadRequest("No medicines provided");
            }

            var result = new ImportResult
            {
                Added = 0,
                Duplicates = 0
            };

            // Get all existing medicine names (converted to lowercase for case-insensitive comparison)
            var existingMedicineNames = await _context.Medicines
                .Select(m => m.Name.ToLower())
                .ToListAsync();

            var medicinesToAdd = new List<Medicine>();

            foreach (var medicine in request.Medicines)
            {
                if (string.IsNullOrWhiteSpace(medicine.Name))
                {
                    continue; // Skip medicines with no name
                }

                // Check for duplicates (case-insensitive)
                if (existingMedicineNames.Contains(medicine.Name.ToLower()))
                {
                    result.Duplicates++;
                    continue;
                }

                // Add to list of medicines to insert
                medicinesToAdd.Add(new Medicine
                {
                    Name = medicine.Name,
                    Packaging = medicine.Packaging,
                    Company = medicine.Company,
                    Composition = medicine.Composition,
                    Note = medicine.Note
                });

                // Add to our tracking list to prevent duplicates within the import batch itself
                existingMedicineNames.Add(medicine.Name.ToLower());
            }

            if (medicinesToAdd.Any())
            {
                await _context.Medicines.AddRangeAsync(medicinesToAdd);
                await _context.SaveChangesAsync();
                result.Added = medicinesToAdd.Count;
            }

            return Ok(result);
        }
    }

    // Move these classes outside of the controller to a separate file
    public class MedicineImportRequest
    {
        public List<MedicineDto> Medicines { get; set; } = new List<MedicineDto>();
    }

    public class MedicineDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Packaging { get; set; }
        public string? Company { get; set; }
        public string? Composition { get; set; }
        public string? Note { get; set; }
    }

    public class ImportResult
    {
        public int Added { get; set; }
        public int Duplicates { get; set; }
    }
}