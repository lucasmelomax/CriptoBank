

using CriptoBank.Domain.Enums;

namespace CriptoBank.Domain.Models
{
    public class WalletHistory
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid WalletId { get; private set; }
        public decimal Amount { get; private set; }
        public TransactionType Type { get; private set; } 
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        protected WalletHistory() { }

        public WalletHistory(Guid walletId, decimal amount, TransactionType type)
        {
            WalletId = walletId;
            Amount = amount;
            Type = type;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
