using CriptoBank.Domain.Models;

namespace CriptoBank.Application.Repositories.Token
{
    public interface IUserTokenRepositories
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken ct);
        Task AddAsync(User user, CancellationToken ct);
    }
}
