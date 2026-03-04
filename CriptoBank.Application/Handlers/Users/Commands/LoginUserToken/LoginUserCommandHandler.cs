using CriptoBank.Application.DTOs.UserToken;
using CriptoBank.Application.Interfaces.UnitOfWork;
using CriptoBank.Application.Repositories.Token;
using MediatR;

namespace CriptoBank.Application.Handlers.Users.Commands.LoginUserToken;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponseDTO>
{
    private readonly IUnitOfWork _uow;
    private readonly IAuthService _authService;

    public LoginUserCommandHandler(IUnitOfWork uow, IAuthService authService)
    {
        _uow = uow;
        _authService = authService;
    }

    public async Task<AuthResponseDTO> Handle(LoginUserCommand request, CancellationToken ct)
    {
        var response = await _authService.LoginAsync(request.LoginDto, ct);

        await _uow.SaveChangesAsync(ct);

        return response;
    }
}