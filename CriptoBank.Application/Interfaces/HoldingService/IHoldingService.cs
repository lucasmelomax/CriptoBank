
using CriptoBank.Application.DTOs.Holdings;
using CriptoBank.Domain.Models;

namespace CriptoBank.Application.Interfaces.HoldingService
{
    public interface IHoldingService
    {
        Task<IEnumerable<Holding>> GetHoldingsUser(Guid portfolioId);
        Task<DashboardDTO> GetDashboard(Guid userId);
    }
}
