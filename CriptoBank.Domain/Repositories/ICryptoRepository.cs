

using CriptoBank.Domain.Models;

namespace CriptoBank.Domain.Repositories
{
    public interface ICryptoRepository
    {
        Task<Crypto?> GetByIdAsync(Guid id);
        Task<Crypto?> GetByNameAsync(string name);
        Task<Crypto?> GetByExternalIdAsync(string externalId);
        Task<IEnumerable<Crypto>> GetAllAsync();
        Task AddAsync(Crypto crypto);
    }
}
