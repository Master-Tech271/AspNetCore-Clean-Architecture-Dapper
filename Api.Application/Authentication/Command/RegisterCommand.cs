using Api.Application.Dtos.Authentication;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Api.Application.Authentication.Command
{
    public class RegisterCommand : IRequest<AuthResponse>
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [MaxLength(13)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
    }
}
