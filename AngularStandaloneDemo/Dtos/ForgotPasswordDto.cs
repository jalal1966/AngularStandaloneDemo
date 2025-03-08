// ForgotPasswordDto.cs
using System.ComponentModel.DataAnnotations;

namespace AngularStandaloneDemo.Dtos
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}

