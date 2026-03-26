

using CriptoBank.Domain.Models;
using CriptoBank.Domain.Repositories;
using CriptoBank.Infrastructure.Context;

namespace CriptoBank.Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly CriptoDbContext _context;

        public WalletRepository(CriptoDbContext context)
        {
            _context = context;
        }

        public async Task Add(Wallet wallet)
        {
            await _context.Wallets.AddAsync(wallet);
        }

        public async Task<Wallet> GetById(Guid userId)
        {
            var user = _context.Wallets.FirstOrDefault(x => x.UserId == userId);
            return user;
        }
    }
}
