

using CriptoBank.Domain.Models;
using CriptoBank.Domain.Repositories;
using CriptoBank.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CriptoBank.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly CriptoDbContext _context;

        public TransactionRepository(CriptoDbContext context)
        {
            _context = context;
        }
        public async Task Add(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
        }

        public async Task<IEnumerable<Transaction>> GetByPortfolioIdAsync(Guid portfolioId)
        {
            return await _context.Transactions
                .Where(t => t.PortfolioId == portfolioId)
                .Include(t => t.Crypto)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByPortfolioAndCryptoAsync(Guid portfolioId, Guid cryptoId)
        {
            return await _context.Transactions
                .Where(t => t.PortfolioId == portfolioId && t.CryptoId == cryptoId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}
