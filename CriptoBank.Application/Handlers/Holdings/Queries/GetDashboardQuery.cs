

using CriptoBank.Application.DTOs.Holdings;
using MediatR;

namespace CriptoBank.Application.Handlers.Holdings.Queries
{
    public record GetDashboardQuery() : IRequest<DashboardDTO>
    {
    }
}
