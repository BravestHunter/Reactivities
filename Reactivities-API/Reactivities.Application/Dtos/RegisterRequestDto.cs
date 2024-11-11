using System.ComponentModel.DataAnnotations;

namespace Reactivities.Application.Dtos
{
    public class RegisterRequestDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string Username { get; set; }

        public string? DisplayName { get; set; }
    }
}
