using System.Security.Cryptography;
using System.Text;

namespace AngularStandaloneDemo.Services
{
    public class PasswordHashService
    {
        public static string GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[16];
                rng.GetBytes(saltBytes);
                return Convert.ToBase64String(saltBytes);
            }
        }

        public static string HashPassword(string password, string salt)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt)))
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = hmac.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool VerifyPassword(string password, string salt, string storedHash)
        {
            string computedHash = HashPassword(password, salt);
            return computedHash == storedHash;
        }
    }
}
