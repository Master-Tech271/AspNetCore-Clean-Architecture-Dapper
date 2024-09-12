using Api.Application.Authentication.Command;
using Api.Application.Authentication.Handler;
using Api.Application.Dtos.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        public AuthController(ISender mediator) : base(mediator)
        {
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AuthResponse authResponse = await _mediator.Send(request);

            return Ok(authResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginQuery request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AuthResponse authResponse = await _mediator.Send(request);

            return Ok(authResponse);
        }
    }
}
