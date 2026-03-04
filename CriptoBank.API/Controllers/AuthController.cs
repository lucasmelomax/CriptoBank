
using CriptoBank.Application.DTOs.User;
using CriptoBank.Application.DTOs.UserToken;
using CriptoBank.Application.Handlers.Users.Commands.LoginUserToken;
using CriptoBank.Application.Handlers.Users.Commands.RegisterUserToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CriptoBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDTO request, CancellationToken ct)
        {
            var command = new RegisterUserCommand(request);
            var result = await _mediator.Send(command, ct);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO request, CancellationToken ct)
        {
            var command = new LoginUserCommand(request);
            var result = await _mediator.Send(command, ct);
            return Ok(result);
        }
    }
}