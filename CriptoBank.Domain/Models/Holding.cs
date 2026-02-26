

namespace CriptoBank.Domain.Models
{
    public class Holding
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid PortfolioId { get; private set; }

        public Guid CryptoId { get; private set; }

        public decimal Quantity { get; private set; }

        public decimal AveragePrice { get; private set; }

        public Portfolio Portfolio { get; private set; } = null!;
        public Crypto Crypto { get; private set; } = null!;

        protected Holding() { }

        public Holding(Guid portfolioId, Guid cryptoId)
        {
            PortfolioId = portfolioId;
            CryptoId = cryptoId;
            Quantity = 0;
            AveragePrice = 0;
        }

        public void UpdatePosition(decimal quantity, decimal unitPrice)
        {
            var totalCost = (Quantity * AveragePrice) + (quantity * unitPrice);
            Quantity += quantity;
            AveragePrice = Quantity == 0 ? 0 : totalCost / Quantity;
        }
    }
}
