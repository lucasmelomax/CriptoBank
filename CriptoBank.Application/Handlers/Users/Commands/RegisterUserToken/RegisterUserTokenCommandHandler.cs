using CriptoBank.Application.DTOs.UserToken;
using CriptoBank.Application.Interfaces.UnitOfWork;
using CriptoBank.Application.Repositories.Token;
using MediatR;

namespace CriptoBank.Application.Handlers.Users.Commands.RegisterUserToken;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponseDTO>
{
    private readonly IUnitOfWork _uow;
    private readonly IAuthService _authService;

    public RegisterUserCommandHandler(IUnitOfWork uow, IAuthService authService)
    {
        _uow = uow;
        _authService = authService;
    }

    public async Task<AuthResponseDTO> Handle(RegisterUserCommand request, CancellationToken ct)
    {

        var response = await _authService.RegisterAsync(request.RegisterDto, ct);

        await _uow.SaveChangesAsync(ct);

        return response;
    }
}