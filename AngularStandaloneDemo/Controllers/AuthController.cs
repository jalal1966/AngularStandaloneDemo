using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Dtos;
using AngularStandaloneDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AngularStandaloneDemo.Models;
using AngularStandaloneDemo.Filters;

namespace AngularStandaloneDemo.Controllers
{
    [Route("api/[controller]")]
    [ServiceFilter(typeof(ValidationActionFilter))]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public AuthController(ApplicationDbContext context, IConfiguration configuration, IAuthService authService)
        {
            _context = context;
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError("", "Username, email, and password are required");
                    return BadRequest(ModelState);
                }

                // Check if passwords match
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Passwords do not match");
                    return BadRequest(ModelState);
                }

                // Check for duplicate username
                if (await _context.Users.AnyAsync(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return BadRequest(ModelState);
                }

                // Check for duplicate email
                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return BadRequest(ModelState);
                }

                // Hash the password
                string salt = PasswordHashService.GenerateSalt();
                string passwordHash = PasswordHashService.HashPassword(model.Password, salt);

                // Create a new user
                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = passwordHash,
                    Salt = salt,
                    FirstName = model.FirstName ?? string.Empty,
                    LastName = model.LastName ?? string.Empty,
                    Address = model.Address ?? string.Empty,
                    TelephoneNo = model.TelephoneNo ?? string.Empty,
                    Salary = model.Salary,
                    Note = model.Note ?? string.Empty,
                    JobTitleID = model.JobTitleID,
                    GenderID = model.GenderID,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    LastLoginAt = DateTime.UtcNow,
                    Specialist = string.Empty // Add this line to set the required 'Specialist' property
                };

                // Save the user to the database
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Registration successful" });
            }
            catch (Exception ex)
            {
                // Log the full exception, including inner exceptions
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner Exception: " + ex.InnerException.Message;
                }
                return StatusCode(500, new { message = "Server Error", errors = new[] { errorMessage } });
            }
        }

        [HttpGet("users/job/{id}")]
        public async Task<IActionResult> GetUsersByJobTitle(int id)
        {
            try
            {
                // Query users with the specified JobTitleId
                var users = await _context.Users
                    .Where(u => u.JobTitleID == id)
                    .Select(u => new
                    {
                        u.UserID,
                        u.Username,
                        u.Email,
                        u.FirstName,
                        u.LastName,
                        u.Address,
                        u.TelephoneNo,
                        u.Salary,
                        u.Note,
                        u.JobTitleID,
                        u.GenderID,
                        u.CreatedAt,
                        u.UpdatedAt,
                        u.LastLoginAt,
                        u.Specialist
                    })
                    .ToListAsync();

                // Check if any users were found
                if (users == null || !users.Any())
                {
                    return NotFound(new { message = "No users found with the specified job title ID" });
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log the full exception, including inner exceptions
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner Exception: " + ex.InnerException.Message;
                }
                return StatusCode(500, new { message = "Server Error", errors = new[] { errorMessage } });
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var controllerType = typeof(UsersController); // If you can reference it
            var location = controllerType.Assembly.Location;
            Console.WriteLine(location);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null || user.Salt == null ||
                !PasswordHashService.VerifyPassword(model.Password, user.Salt, user.PasswordHash))
                return Unauthorized("Invalid username or password");

            // Update last login time
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                Username = user.Username,
                Expiration = DateTime.UtcNow.AddHours(1),
                JobTitleId = user.JobTitleID, // Include the job title ID
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserID =user.UserID
    });
        }

        private string GenerateJwtToken(User user)
        {
            var secret = _configuration["Jwt:Secret"];
            if (string.IsNullOrEmpty(secret))
            {
                throw new InvalidOperationException("JWT Secret is not configured properly.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            await _authService.ForgotPasswordAsync(model.Email);

            // Always return OK even if email doesn't exist for security reasons
            return Ok(new { message = "If your email exists in our system, you will receive a password reset link shortly." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (!await _authService.ValidateEmailTokenAsync(model.Email, model.Token))
                return BadRequest(new { message = "Invalid token" });

            var result = await _authService.ResetPasswordAsync(model.Email, model.Token, model.Password);

            if (!result)
                return BadRequest(new { message = "Password reset failed" });

            return Ok(new { message = "Password has been reset successfully" });
        }

        [HttpPost("validate-reset-token")]
        public async Task<IActionResult> ValidateResetToken([FromBody] ValidateTokenDto model)
        {
            var isValid = await _authService.ValidateEmailTokenAsync(model.Email, model.Token);
            return Ok(new { isValid });
        }
    }
}