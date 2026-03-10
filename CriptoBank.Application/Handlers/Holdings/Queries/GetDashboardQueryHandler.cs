

using System.Security.Claims;
using CriptoBank.Application.DTOs.Holdings;
using CriptoBank.Application.Interfaces.HoldingService;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CriptoBank.Application.Handlers.Holdings.Queries
{
    public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardDTO>
    {
        private readonly IHoldingService _holdingService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetDashboardQueryHandler(IHoldingService holdingService, IHttpContextAccessor httpContextAccessor)
        {
            _holdingService = holdingService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DashboardDTO> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("Usuário não autenticado.");

            if (!Guid.TryParse(userIdClaim, out var userId))
                throw new Exception("ID de usuário inválido no token.");

            return await _holdingService.GetDashboard(userId);
        }
    }
}
