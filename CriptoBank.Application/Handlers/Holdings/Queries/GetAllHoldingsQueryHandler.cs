
using System.Security.Claims;
using AutoMapper;
using CriptoBank.Application.DTOs.Holdings;
using CriptoBank.Application.Interfaces.CoinService;
using CriptoBank.Application.Interfaces.HoldingService;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CriptoBank.Application.Handlers.Holdings.Queries
{
    public class GetAllHoldingsQueryHandler : IRequestHandler<GetAllHoldingsQuery, IEnumerable<HoldingDTO>>
    {
        private readonly IHoldingService _holdingService;
        private readonly ICoinService _coinService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllHoldingsQueryHandler(
            IHoldingService holdingService,
            ICoinService coinService,
            IHttpContextAccessor httpContextAccessor)
        {
            _holdingService = holdingService;
            _coinService = coinService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<HoldingDTO>> Handle(GetAllHoldingsQuery request, CancellationToken ct)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("Usuário não autenticado.");

            var userId = Guid.Parse(userIdClaim);

            var holdings = await _holdingService.GetHoldingsUser(userId);

            var ids = string.Join(",", holdings.Select(h => h.Crypto.ExternalId));
            var prices = await _coinService.GetCoinDataAsync(ids);

            var listaDto = holdings.Select(item => new HoldingDTO
            {
                Symbol = item.Crypto.Symbol,
                Quantity = item.Quantity,
                AveragePrice = item.AveragePrice,
                CurrentPrice = prices?.FirstOrDefault(p => p.Id == item.Crypto.ExternalId)?.Current_Price ?? 0
            });

            return listaDto;
        }
    }
}
