
using CriptoBank.Domain.Models;
using CriptoBank.Domain.Repositories;
using CriptoBank.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CriptoBank.Infrastructure.Repositories
{
    public class CryptoRepository : ICryptoRepository
    {
        private readonly CriptoDbContext _context;

        public CryptoRepository(CriptoDbContext context)
        {
            _context = context;
        }

        public async Task<Crypto?> GetByIdAsync(Guid id)
        {
            return await _context.Cryptos.FindAsync(id);
        }

        public async Task<Crypto?> GetByExternalIdAsync(string externalId)
        {
            return await _context.Cryptos
                .FirstOrDefaultAsync(c => c.ExternalId == externalId);
        }

        public async Task<IEnumerable<Crypto>> GetAllAsync()
        {
            return await _context.Cryptos
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task AddAsync(Crypto crypto)
        {
            await _context.Cryptos.AddAsync(crypto);
        }
    }
}
