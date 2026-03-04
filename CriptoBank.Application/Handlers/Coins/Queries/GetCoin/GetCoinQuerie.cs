
using CriptoBank.Application.DTOs.Crypto;
using MediatR;

namespace CriptoBank.Application.Handlers.Coins.Queries.GetCoin
{
    public record GetCoinQuerie(string nome) : IRequest<CoinMarketDto>
    {
    }
}
