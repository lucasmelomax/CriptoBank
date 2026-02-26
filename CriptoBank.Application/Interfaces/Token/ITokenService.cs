using CriptoBank.Domain.Models;

namespace CriptoBank.Application.Repositories.Token
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}