

using CriptoBank.Application.DTOs.Holdings;
using CriptoBank.Application.Interfaces.CoinService;
using CriptoBank.Application.Interfaces.HoldingService;
using CriptoBank.Domain.Models;
using CriptoBank.Domain.Repositories;

namespace CriptoBank.Application.Services
{
    public class HoldingService : IHoldingService
    {

        private readonly IHoldingRepository _holdingRepository;
        private readonly ICoinService _coinService;

        public HoldingService(IHoldingRepository holdingRepository, ICoinService coinService)
        {
            _holdingRepository = holdingRepository;
            _coinService = coinService;
        }

        public async Task<DashboardDTO> GetDashboard(Guid userId)
        {
            var holdings = await _holdingRepository.GetAllHoldingsByPortfolio(userId);
            var dashboard = new DashboardDTO();

            var externalIds = holdings
                .Select(h => h.Crypto.ExternalId)
                .Distinct()
                .ToList();

            var allCoinsData = await _coinService.GetCoinsDataAsync(externalIds);

            foreach (var item in holdings)
            {
                var coinInfo = allCoinsData.FirstOrDefault(c =>
                    c.Id.Equals(item.Crypto.ExternalId, StringComparison.OrdinalIgnoreCase));

                var currentPrice = coinInfo?.Current_Price ?? item.AveragePrice;

                dashboard.TotalInvested += (item.Quantity * item.AveragePrice);
                dashboard.TotalBalance += (item.Quantity * currentPrice);
            }

            return dashboard;
        }

        public async Task<IEnumerable<HoldingDTO>> GetHoldingsWithPrices(Guid userId)
        {
            var holdings = await _holdingRepository.GetAllHoldingsByPortfolio(userId);

            if (holdings == null || !holdings.Any())
                return Enumerable.Empty<HoldingDTO>();

            var externalIds = holdings.Select(h => h.Crypto.ExternalId).Distinct().ToList();
            var prices = await _coinService.GetCoinsDataAsync(externalIds);

            return holdings.Select(item => new HoldingDTO
            {
                Symbol = item.Crypto.Symbol,
                Quantity = item.Quantity,
                AveragePrice = item.AveragePrice,
                CurrentPrice = prices?.FirstOrDefault(p => p.Id == item.Crypto.ExternalId)?.Current_Price ?? 0
            });
        }
    }
}
