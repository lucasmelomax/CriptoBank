
using MediatR;

namespace CriptoBank.Application.Handlers.SellCrypto
{
    public record SellCryptoCommand(string cryptoName, decimal Quantity) : IRequest<bool>
    {
    }
}
