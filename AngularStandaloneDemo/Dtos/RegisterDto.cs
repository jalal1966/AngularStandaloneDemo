using System.ComponentModel.DataAnnotations;

namespace AngularStandaloneDemo.Dtos
{
    public class RegisterDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? TelephoneNo { get; set; }
        public decimal Salary { get; set; }
        public string? Note { get; set; }
        public int JobTitleID { get; set; } // Added property to fix CS1061
        public int GenderID { get; set; } // Added property to fix CS1061
    }
}
