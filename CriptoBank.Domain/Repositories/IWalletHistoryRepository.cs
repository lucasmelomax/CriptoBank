
using CriptoBank.Domain.Models;

namespace CriptoBank.Domain.Repositories
{
    public interface IWalletHistoryRepository
    {
        Task Add(WalletHistory walletHistory);
        Task<IEnumerable<WalletHistory>> GetAllWalletHistories(Guid walletId);
    }
}
