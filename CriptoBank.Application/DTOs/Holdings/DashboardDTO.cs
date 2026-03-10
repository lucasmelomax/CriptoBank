

namespace CriptoBank.Application.DTOs.Holdings
{
    public class DashboardDTO
    {
        private decimal _totalBalance;
        public decimal TotalBalance
        {
            get => Math.Round(_totalBalance, 2);
            set => _totalBalance = value;
        }

        private decimal _totalInvested;
        public decimal TotalInvested
        {
            get => Math.Round(_totalInvested, 2);
            set => _totalInvested = value;
        }
        public decimal TotalProfitLoss => Math.Round(TotalBalance - TotalInvested, 2);

        public decimal TotalProfitLossPercentage => TotalInvested > 0
            ? Math.Round((TotalProfitLoss / TotalInvested) * 100, 2)
            : 0;

        public bool IsInProfit => TotalProfitLoss >= 0;

    }
}
