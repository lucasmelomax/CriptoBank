

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

            foreach(var item in holdings)
            {
                var coinData = await _coinService.GetCoinDataAsync(item.Crypto.ExternalId);

                var currentPrice = coinData.FirstOrDefault()?.Current_Price ?? 0;

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
