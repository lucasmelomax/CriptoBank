

using CriptoBank.Domain.Models;
using CriptoBank.Domain.Repositories;
using CriptoBank.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;


namespace CriptoBank.Infrastructure.Repositories
{
    public class HoldingRepository : IHoldingRepository
    {
        private readonly CriptoDbContext _context;

        public HoldingRepository(CriptoDbContext context)
        {
            _context = context;
        }

        public async Task Add(Holding holding)
        {
            await _context.Holdings.AddAsync(holding);
        }

        public async Task<Holding> GetByCrypto(Guid portfolioId, Guid cryptoId)
        {
            var holding = await _context.Holdings
            .FirstOrDefaultAsync(h => h.PortfolioId == portfolioId && h.CryptoId == cryptoId);

            return holding;
        }
    }
}
