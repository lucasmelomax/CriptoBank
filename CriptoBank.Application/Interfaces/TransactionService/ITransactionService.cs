

using CriptoBank.Application.DTOs.Transaction;

namespace CriptoBank.Application.Interfaces.TransactionService
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDTO>> GetTransactions(Guid UserId);
    }
}
