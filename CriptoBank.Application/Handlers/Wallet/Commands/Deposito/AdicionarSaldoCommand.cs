using MediatR;

namespace CriptoBank.Application.Handlers.Wallet.Commands.Deposito
{
    public record AdicionarSaldoCommand(decimal saldo) : IRequest<bool>
    {
    }
}
