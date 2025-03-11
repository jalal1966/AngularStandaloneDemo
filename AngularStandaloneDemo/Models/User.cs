// Models/User.cs
using System.ComponentModel.DataAnnotations;

namespace AngularStandaloneDemo.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; } // Explicitly define the primary key
        public required string Username { get; set; }
        [Required]
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public string? Salt { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Address { get; set; }
        public required string TelephoneNo { get; set; }
        public decimal? Salary { get; set; }
        public string? Note { get; set; } // Made nullable to fix CS8618
        public int JobTitleID { get; set; }
        public int GenderID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime LastLoginAt { get; set; }
    }

}