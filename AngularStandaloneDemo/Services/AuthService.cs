using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AngularStandaloneDemo.Data;
using AngularStandaloneDemo.Dtos;
using AngularStandaloneDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace AngularStandaloneDemo.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<bool> RegisterAsync(RegisterDto registerDto);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
        Task<bool> ValidateEmailTokenAsync(string email, string token);
    }

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly Dictionary<string, string> _passwordResetTokens = new Dictionary<string, string>();

        public AuthService(
            ApplicationDbContext context,
            TokenService tokenService,
            IEmailService emailService)
        {
            _context = context;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null)
                throw new Exception("Invalid username or password");

            if (user.Salt == null || !VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.Salt))
                throw new Exception("Invalid username or password");

            // Update last login time
            user.LastLoginAt = DateTime.Now;
            await _context.SaveChangesAsync();

            // Generate JWT token
            var token = _tokenService.GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Username = user.Username,
                Expiration = DateTime.UtcNow.AddHours(3), // Match token expiration in TokenService
            };
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
                throw new Exception("Username is already taken");

            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                throw new Exception("Email is already registered");

            CreatePasswordHash(registerDto.Password, out string passwordHash, out string salt);

#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                Salt = salt,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Specialist = registerDto.Specialist,
                Address = registerDto.Address,
                TelephoneNo = registerDto.TelephoneNo,
                Salary = registerDto.Salary,
                Note = registerDto.Note,
                CreatedAt = DateTime.Now
            };
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8601 // Possible null reference assignment.

            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return false;

            // Generate a reset token
            string token = GenerateResetToken();
            _passwordResetTokens[email] = token;

            // Send email with the token
            string resetLink = $"http://localhost:4200/reset-password?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";
            string emailBody = $"Please click the following link to reset your password: {resetLink}";

            await _emailService.SendEmail(email, "Password Reset", emailBody);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            if (!_passwordResetTokens.TryGetValue(email, out var storedToken) || storedToken != token)
                return false;

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return false;

            CreatePasswordHash(newPassword, out string passwordHash, out string salt);

            user.PasswordHash = passwordHash;
            user.Salt = salt;
            user.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            // Remove the used token
            _passwordResetTokens.Remove(email);

            return true;
        }

        public Task<bool> ValidateEmailTokenAsync(string email, string token)
        {
            return Task.FromResult(
                _passwordResetTokens.TryGetValue(email, out var storedToken) &&
                storedToken == token);
        }

        private static void CreatePasswordHash(string password, out string passwordHash, out string salt)
        {
            using var hmac = new HMACSHA512();
            salt = Convert.ToBase64String(hmac.Key);
            passwordHash = Convert.ToBase64String(
                hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        private static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            using var hmac = new HMACSHA512(saltBytes);
            var computedHash = Convert.ToBase64String(
                hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return computedHash == storedHash;
        }

        private static string GenerateResetToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
