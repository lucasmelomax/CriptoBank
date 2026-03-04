

using CriptoBank.Domain.Enums;

namespace CriptoBank.Application.Interfaces.BuyService
{
    public interface ICryptoTransactionService
    {
        Task BuyAsync(Guid userId, Guid cryptoId, TransactionType type, decimal quantity, decimal unitPrice, CancellationToken ct);
    }
}
