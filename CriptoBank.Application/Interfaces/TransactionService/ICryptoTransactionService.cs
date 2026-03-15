

using CriptoBank.Domain.Enums;

namespace CriptoBank.Application.Interfaces.BuyService
{
    public interface ICryptoTransactionService
    {
        Task BuyAsync(Guid userId, string cryptoName, decimal quantity, CancellationToken ct);
        Task SellAsync(Guid userId, string cryptoName, decimal quantity, CancellationToken ct);
    }
}
