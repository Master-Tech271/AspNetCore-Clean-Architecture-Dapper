using Api.Application.Dtos.Authentication;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Application.Authentication.Handler
{
    public class LoginQuery : IRequest<AuthResponse>
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = default!;
        [Required]
        [MaxLength(13)]
        public string Password { get; set; } = default!;
    }
}
