using CriptoBank.Application.DTOs.User;
using CriptoBank.Application.DTOs.UserToken;
using MediatR;

namespace CriptoBank.Application.Handlers.Users.Commands.LoginUserToken;

public class LoginUserCommand : IRequest<AuthResponseDTO>
{
    public LoginRequestDTO LoginDto { get; }

    public LoginUserCommand(LoginRequestDTO loginDto)
    {
        LoginDto = loginDto;
    }
}