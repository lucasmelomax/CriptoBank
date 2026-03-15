
using CriptoBank.Application.DTOs.Holdings;
using CriptoBank.Application.Interfaces.HoldingService;
using CriptoBank.Application.Interfaces.Token;
using MediatR;

namespace CriptoBank.Application.Handlers.Holdings.Queries
{
    public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardDTO>
    {
        private readonly IHoldingService _holdingService;
        private readonly ICurrentUserService _currentUserService;

        public GetDashboardQueryHandler(IHoldingService holdingService, ICurrentUserService currentUserService)
        {
            _holdingService = holdingService;
            _currentUserService = currentUserService;
        }

        public async Task<DashboardDTO> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Usuário não identificado.");

            return await _holdingService.GetDashboard(userId);
        }
    }
}
