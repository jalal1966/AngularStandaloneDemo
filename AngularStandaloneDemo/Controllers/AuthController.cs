using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Dtos;
using AngularStandaloneDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AngularStandaloneDemo.Models;



namespace AngularStandaloneDemo.Controllers
{
    [Route("api/[controller]")]
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
            if (model.Username == null || model.Email == null || model.Password == null)
                return BadRequest("Username, email, and password are required");

            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Username == model.Username || u.Email == model.Email))
                return BadRequest("Username or email already exists");

            // Generate salt and hash password
            string salt = PasswordHashService.GenerateSalt();
            string passwordHash = PasswordHashService.HashPassword(model.Password, salt);

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = passwordHash,
                Salt = salt,
                FirstName = model.FirstName ?? string.Empty, // Fix for CS8601
                LastName = model.LastName ?? string.Empty, // Fix for CS8601
                Address = model.Address ?? string.Empty, // Fix for CS8601
                TelephoneNo = model.TelephoneNo ?? string.Empty, // Fix for CS8601
                Salary = model.Salary, // Fix for CS8601
                Note = model.Note ?? string.Empty, // Fix for CS8601
                JobTitleID = model.JobTitleID, // Fix for CS860
                GenderID = model.GenderID, // Fix for CS860
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
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
                Expiration = DateTime.UtcNow.AddHours(1),
                Username = user.Username
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