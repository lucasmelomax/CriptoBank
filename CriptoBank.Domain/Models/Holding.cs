

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

        public void AddPurchase(decimal newQuantity, decimal newPrice)
        {
            if (newQuantity <= 0) throw new ArgumentException("Quantidade deve ser positiva.");

            decimal currentTotalCost = Quantity * AveragePrice;
            decimal newPurchaseCost = newQuantity * newPrice;

            decimal totalQuantity = Quantity + newQuantity;
            decimal totalCost = currentTotalCost + newPurchaseCost;

            AveragePrice = totalCost / totalQuantity;
            Quantity = totalQuantity;
        }

        public void RemoveBalance(decimal quantity)
        {
            if (quantity > Quantity) throw new ArgumentException("Saldo insuficiente.");
            Quantity -= quantity;
        }

    }
}
