
using CriptoBank.Domain.Interfaces;
using CriptoBank.Infrastructure.Context;

namespace CriptoBank.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CriptoDbContext _context;
        //public IPortfolioRepository Portfolios { get; }
        public UnitOfWork(CriptoDbContext context) //,IPortfolioRepository portfolios
        {
            _context = context;
            //Portfolios = portfolios;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
