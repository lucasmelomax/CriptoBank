
namespace CriptoBank.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        //IPortfolioRepository Portfolios { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
