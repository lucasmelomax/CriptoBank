

using CriptoBank.Application.DTOs.User;
using CriptoBank.Application.DTOs.UserToken;

namespace CriptoBank.Application.Repositories.Token
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO request, CancellationToken ct);
        Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request, CancellationToken ct);
    }
}
