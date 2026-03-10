

namespace CriptoBank.Application.DTOs.Holdings
{
    public class DashboardDTO
    {
        public decimal TotalBalance { get; set; }
        public decimal TotalInvested { get; set; }

        public decimal TotalProfitLoss => TotalBalance - TotalInvested;

        public decimal TotalProfitLossPercentage => TotalInvested > 0
            ? (TotalProfitLoss / TotalInvested) * 100
            : 0;
        public bool IsInProfit => TotalProfitLoss >= 0;

    }
}
