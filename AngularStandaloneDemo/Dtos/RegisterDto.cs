using System.ComponentModel.DataAnnotations;

namespace AngularStandaloneDemo.Dtos
{
    public class RegisterDto
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(5)]
        public required string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? TelephoneNo { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "9999999999", ErrorMessage = "The value for {0} must be between {1} and {2}.")]
        public decimal Salary { get; set; }

        public string? Note { get; set; }
        public int JobTitleID { get; set; } // Added property to fix CS1061
        public int GenderID { get; set; } // Added property to fix CS1061
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}