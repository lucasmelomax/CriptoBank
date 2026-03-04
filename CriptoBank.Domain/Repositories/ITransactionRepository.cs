

using CriptoBank.Domain.Models;

namespace CriptoBank.Domain.Repositories
{
    public interface ITransactionRepository
    {
        Task Add(Transaction transaction);
        Task<IEnumerable<Transaction>> GetByPortfolioIdAsync(Guid portfolioId);
        Task<IEnumerable<Transaction>> GetByPortfolioAndCryptoAsync(Guid portfolioId, Guid cryptoId);
    }
}
