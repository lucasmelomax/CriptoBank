using CriptoBank.Application.Interfaces.UnitOfWork;

using CriptoBank.Infrastructure.Context;

namespace CriptoBank.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CriptoDbContext _context;

        public UnitOfWork(CriptoDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}