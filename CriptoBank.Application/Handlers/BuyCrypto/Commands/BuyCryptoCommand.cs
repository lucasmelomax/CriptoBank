
using MediatR;

namespace CriptoBank.Application.Handlers.BuyCrypto.Commands
{
    public record BuyCryptoCommand
    (

        Guid CryptoId,
        decimal Quantity

    ) : IRequest<bool>;
    
}

