

using CriptoBank.Domain.Enums;

namespace CriptoBank.Domain.Models
{
    public class Transaction
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid PortfolioId { get; private set; }

        public Guid CryptoId { get; private set; }

        public TransactionType Type { get; private set; }

        public decimal Quantity { get; private set; }

        public decimal UnitPrice { get; private set; }

        public decimal TotalValue { get; private set; }
        
        public DateTime TransactionDate { get; private set; }

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;


        public Portfolio Portfolio { get; private set; } = null!;
        public Crypto Crypto { get; private set; } = null!;

        protected Transaction() { }

        public Transaction(Guid portfolioId, Guid cryptoId,
            TransactionType type, decimal quantity, decimal unitPrice)
        {
            PortfolioId = portfolioId;
            CryptoId = cryptoId;
            Type = type;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalValue = quantity * unitPrice;
            TransactionDate = DateTime.UtcNow;
        }
           
    }
}
