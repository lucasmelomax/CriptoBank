using CriptoBank.Application.DTOs.UserToken;
using MediatR;

namespace CriptoBank.Application.Handlers.Users.Commands.RegisterUserToken;

public class RegisterUserCommand : IRequest<AuthResponseDTO>
{
    public RegisterRequestDTO RegisterDto { get; }

    public RegisterUserCommand(RegisterRequestDTO registerDto)
    {
        RegisterDto = registerDto;
    }
}