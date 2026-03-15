
using CriptoBank.Application.DTOs.Holdings;
using CriptoBank.Application.Interfaces.HoldingService;
using CriptoBank.Application.Interfaces.Token;
using MediatR;


namespace CriptoBank.Application.Handlers.Holdings.Queries
{
    public class GetAllHoldingsQueryHandler : IRequestHandler<GetAllHoldingsQuery, IEnumerable<HoldingDTO>>
    {
        private readonly IHoldingService _holdingService;
        private readonly ICurrentUserService _currentUserService;

        public GetAllHoldingsQueryHandler(IHoldingService holdingService, ICurrentUserService currentUserService)
        {
            _holdingService = holdingService;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<HoldingDTO>> Handle(GetAllHoldingsQuery request, CancellationToken ct)
        {

            var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Usuário não identificado.");

            var result = await _holdingService.GetHoldingsWithPrices(userId);

            return result;
        }
    }
}
