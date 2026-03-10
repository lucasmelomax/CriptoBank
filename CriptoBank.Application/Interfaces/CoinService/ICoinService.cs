using CriptoBank.Application.DTOs.Crypto;

namespace CriptoBank.Application.Interfaces.CoinService;

public interface ICoinService
{
    Task<List<CoinMarketDto>> GetAllCoinsAsync();
    Task<List<CoinMarketDto?>> GetCoinDataAsync(string externalId);
    Task SyncCryptosAsync();
}