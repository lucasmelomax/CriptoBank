
using CriptoBank.Application.DTOs.Wallet;
using MediatR;

namespace CriptoBank.Application.Handlers.Wallet.Queries.GetSaldo
{
    public record GetSaldoWalletQuery() : IRequest<WalletSaldoDTO>
    {
    }
}
