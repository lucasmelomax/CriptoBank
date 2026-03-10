
namespace CriptoBank.Application.DTOs.Holdings
{
    public class HoldingDTO
    {
        public string Symbol { get; set; }

        private decimal _quantity;
        public decimal Quantity
        {
            get => Math.Round(_quantity, 4);
            set => _quantity = value;
        }

        private decimal _averagePrice;
        public decimal AveragePrice
        {
            get => Math.Round(_averagePrice, 2);
            set => _averagePrice = value;
        }

        private decimal _currentPrice;
        public decimal CurrentPrice
        {
            get => Math.Round(_currentPrice, 2);
            set => _currentPrice = value;
        }
        public decimal TotalInvested => Math.Round(Quantity * AveragePrice, 2);

        public decimal CurrentValue => Math.Round(Quantity * CurrentPrice, 2);

        public decimal ProfitLoss => Math.Round(CurrentValue - TotalInvested, 2);

        public decimal ProfitLossPercentage => AveragePrice > 0
            ? Math.Round(((CurrentPrice - AveragePrice) / AveragePrice) * 100, 2)
            : 0;
    }
}
