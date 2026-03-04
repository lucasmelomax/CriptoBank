
using CriptoBank.Domain.Models;

namespace CriptoBank.Domain.Repositories
{
    public interface IPortfolioRepository
    {
        Task<Portfolio?> GetByUserIdAsync(Guid userId);
        Task AddAsync(Portfolio portfolio);
        Task<Portfolio?> GetWithHoldingsAsync(Guid userId);
    }
}
