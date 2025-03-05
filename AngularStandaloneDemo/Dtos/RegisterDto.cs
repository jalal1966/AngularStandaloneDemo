using System.ComponentModel.DataAnnotations;

namespace AngularStandaloneDemo.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MinLength(5)]
        public string? Password { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? TelephoneNo { get; set; }
       
        public decimal Salary { get; set; } = 0m; // Fix: Changed to decimal type and initialized to 0
       
        public string? Note { get; set; }

    }
}
