

using CriptoBank.Domain.Models;
using CriptoBank.Domain.Repositories;
using CriptoBank.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CriptoBank.Infrastructure.Repositories
{
    public class WalletHistoryRepository : IWalletHistoryRepository
    {
        private readonly CriptoDbContext _context;

        public WalletHistoryRepository(CriptoDbContext context)
        {
            _context = context;
        }

        public async Task Add(WalletHistory walletHistory)
        {
            await _context.WalletHistories.AddAsync(walletHistory);
        }

        public async Task<IEnumerable<WalletHistory>> GetAllWalletHistories(Guid walletId)
        {
            return await _context.WalletHistories
                .Where(x => x.WalletId == walletId) 
                .OrderByDescending(x => x.CreatedAt) 
                .ToListAsync();
        }
    }
    }

