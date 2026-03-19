
using CriptoBank.Domain.Models;
using CriptoBank.Domain.Repositories;
using CriptoBank.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CriptoBank.Infrastructure.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly CriptoDbContext _context;

        public PortfolioRepository(CriptoDbContext context)
        {
            _context = context;
        }

        public async Task<Portfolio?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Portfolios
        .Include(p => p.Holdings) 
        .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<Portfolio?> GetWithHoldingsAsync(Guid userId)
        {
            return await _context.Portfolios
                .Include(p => p.Holdings)
                    .ThenInclude(h => h.Crypto)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task AddAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
        }
    }
}
