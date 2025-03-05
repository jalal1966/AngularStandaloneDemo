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
    }
}
