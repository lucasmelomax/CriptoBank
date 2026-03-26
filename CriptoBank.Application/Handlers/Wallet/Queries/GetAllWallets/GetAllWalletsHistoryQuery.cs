using CriptoBank.Application.DTOs.Wallet;
using MediatR;

namespace CriptoBank.Application.Handlers.Wallet.Queries.GetAll
{
    public record GetAllWalletsHistoryQuery() : IRequest<IEnumerable<WalletDTO>>
    {
    }
}
