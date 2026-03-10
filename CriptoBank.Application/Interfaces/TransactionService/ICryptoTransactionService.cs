

using CriptoBank.Domain.Enums;

namespace CriptoBank.Application.Interfaces.BuyService
{
    public interface ICryptoTransactionService
    {
        Task BuyAsync(Guid userId, string cryptoName, TransactionType type, decimal quantity, decimal unitPrice, CancellationToken ct);
        Task SellAsync(Guid userId, string cryptoName, TransactionType type, decimal quantity, decimal unitPrice, CancellationToken ct);
    }
}
