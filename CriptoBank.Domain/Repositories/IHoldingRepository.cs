
using CriptoBank.Domain.Models;

namespace CriptoBank.Domain.Repositories
{
    public interface IHoldingRepository
    {
        Task Add(Holding holding);
        Task<Holding> GetByCrypto(Guid portfolioId, Guid cryptoId);
    }
}
