
using CriptoBank.Application.DTOs.Crypto;
using CriptoBank.Application.Interfaces.CoinService;
using MediatR;

namespace CriptoBank.Application.Handlers.Coins.Queries.GetCoin
{
    public class GetCoinQuerieHandler : IRequestHandler<GetCoinQuerie, CoinMarketDto>
    {
        private readonly ICoinService _cryptoService;

        public GetCoinQuerieHandler(ICoinService cryptoService)
        {
            _cryptoService = cryptoService;
        }
        public async Task<CoinMarketDto> Handle(GetCoinQuerie request, CancellationToken cancellationToken)
        {
            var ids =request.nome ;

            var coins = await _cryptoService.GetCoinDataAsync(ids);

            return coins ?? new CoinMarketDto();
        }
    }
}
