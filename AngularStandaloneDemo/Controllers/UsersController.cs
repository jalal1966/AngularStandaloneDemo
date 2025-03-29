using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Enums;
using AngularStandaloneDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularStandaloneDemo.Controllers
{
    // --- UserController for doctors and nurses ---
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
         // TO DO
        [HttpGet("Administrator")]
        public async Task<ActionResult<IEnumerable<User>>> GetAdministrator()
        {
            return await _context.Users
                .Where(u => (int)u.JobTitleID == (int)JobTitleID.Admin) 
                .ToListAsync();
        }

        [HttpGet("doctors")]
        public async Task<ActionResult<IEnumerable<User>>> GetDoctors()
        {
            return await _context.Users
               .Where(u => (int)u.JobTitleID == (int)JobTitleID.Doctor) // Cast both sides to int
                .ToListAsync();
        }
        [HttpGet("nurses")]
        public async Task<ActionResult<IEnumerable<User>>> GetNurses()
        {
            return await _context.Users
                .Where(u => (int)u.JobTitleID == (int)JobTitleID.Nurse)
                .ToListAsync();
        }

        [HttpGet("Management")]
        public async Task<ActionResult<IEnumerable<User>>> GetManagement()
        {
            return await _context.Users
                .Where(u => (int)u.JobTitleID == (int)JobTitleID.Management)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.UserID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}


