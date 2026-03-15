
using MediatR;

namespace CriptoBank.Application.Handlers.Coins.Commands
{
    public record SyncCryptosCommand() : IRequest<bool>
    {
    }
}
