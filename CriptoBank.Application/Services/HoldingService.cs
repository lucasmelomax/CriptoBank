

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

        public async Task<IEnumerable<Holding>> GetHoldingsUser(Guid userId)
        {

            var holdings = await _holdingRepository.GetAllHoldingsByPortfolio(userId);

            if (holdings == null || !holdings.Any())
            {
                throw new KeyNotFoundException("Nenhum investimento encontrado para este portfólio.");
            }

            return holdings;
        }
    }
}
