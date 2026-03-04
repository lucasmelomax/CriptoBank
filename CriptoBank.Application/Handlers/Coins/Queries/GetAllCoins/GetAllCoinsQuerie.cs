using MediatR;
using CriptoBank.Application.DTOs.Crypto;


namespace CriptoBank.Application.Handlers.Coins.Queries.GetAllCoins;

public record GetAllCoinsQuerie() : IRequest<List<CoinMarketDto>>
{
}