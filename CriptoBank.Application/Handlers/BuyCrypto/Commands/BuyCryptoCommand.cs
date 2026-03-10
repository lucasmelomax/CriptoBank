
using MediatR;

namespace CriptoBank.Application.Handlers.BuyCrypto.Commands
{
    public record BuyCryptoCommand
    (

        string cryptoName,
        decimal Quantity

    ) : IRequest<bool>;
    
}

