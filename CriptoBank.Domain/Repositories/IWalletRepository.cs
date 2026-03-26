
using CriptoBank.Domain.Models;

namespace CriptoBank.Domain.Repositories
{
    public interface IWalletRepository
    {

        Task<Wallet> GetById(Guid userId);
        Task Add(Wallet wallet);

    }
}
