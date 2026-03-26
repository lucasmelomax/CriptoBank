
using MediatR;

namespace CriptoBank.Application.Handlers.Wallet.Commands.Saque
{
    public record SaqueCommand(decimal valor) : IRequest<bool>
    {

    }
}
