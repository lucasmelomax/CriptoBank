using MediatR;
using CriptoBank.Application.DTOs.Crypto;
using CriptoBank.Application.Interfaces.CoinService;

namespace CriptoBank.Application.Handlers.Coins.Queries.GetAllCoins;

public class GetAllCoinsQuerieHandler : IRequestHandler<GetAllCoinsQuerie, List<CoinMarketDto>>
{
    private readonly ICoinService _cryptoService;

    public GetAllCoinsQuerieHandler(ICoinService cryptoService)
    {
        _cryptoService = cryptoService;
    }

    public async Task<List<CoinMarketDto>> Handle(GetAllCoinsQuerie request, CancellationToken cancellationToken)
    {
        return await _cryptoService.GetAllCoinsAsync();
    }
}