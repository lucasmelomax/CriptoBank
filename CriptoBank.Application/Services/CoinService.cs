

using System.Net.Http.Json;
using System.Text.Json;
using CriptoBank.Application.DTOs.Crypto;
using CriptoBank.Application.Interfaces.CoinService;
using CriptoBank.Domain.Models;
using CriptoBank.Domain.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace CriptoBank.Application.Services;

public class CoinService : ICoinService
{
    private readonly HttpClient _http;
    private readonly ICryptoRepository _cryptoRepository;
    private readonly IMemoryCache _cache;

    public CoinService(HttpClient http, ICryptoRepository cryptoRepository, IMemoryCache cache)
    {
        _cryptoRepository = cryptoRepository;
        _http = http;
        _cache = cache;

        if (!_http.DefaultRequestHeaders.Contains("User-Agent"))
        {
            _http.DefaultRequestHeaders.Add("User-Agent", "CriptoBankApp/1.0");
        }
    }

    public async Task<List<CoinMarketDto>> GetAllCoinsAsync()
    {
        var url = $"https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=250&page=1";

        try
        {
            var coins = await _http.GetFromJsonAsync<List<CoinMarketDto>>(url);
            return coins ?? new List<CoinMarketDto>();
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Erro ao acessar Coin API: {ex.Message}");
        }
    }

    public async Task<List<CoinMarketDto>> GetCoinsDataAsync(List<string> externalIds)
    {
        if (externalIds == null || !externalIds.Any()) return new List<CoinMarketDto>();

        var result = new List<CoinMarketDto>();
        var idsToFetch = new List<string>();

        foreach (var id in externalIds)
        {
            if (_cache.TryGetValue($"Price_{id.ToLower()}", out CoinMarketDto cachedCoin))
            {
                result.Add(cachedCoin!);
            }
            else
            {
                idsToFetch.Add(id.ToLower());
            }
        }

        if (idsToFetch.Any())
        {
            var idsQuery = string.Join(",", idsToFetch);
            var url = $"https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&ids={idsQuery}";

            var response = await _http.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var coinsFromApi = JsonSerializer.Deserialize<List<CoinMarketDto>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<CoinMarketDto>();

                foreach (var coin in coinsFromApi)
                {
                    _cache.Set($"Price_{coin.Id.ToLower()}", coin, TimeSpan.FromMinutes(1));

                    if (!result.Any(r => r.Id == coin.Id))
                        result.Add(coin);
                }
            }
        }

        return result;
    }

    public async Task<CoinMarketDto?> GetCoinDataAsync(string externalId)
    {
        var list = await GetCoinsDataAsync(new List<string> { externalId });
        var primeiro = list.FirstOrDefault();
        if(primeiro is null) throw new KeyNotFoundException($"Erro ao achar moeda.");
        return primeiro;
    }
    public async Task SyncCryptosAsync()
    {
        var externalCoins = await GetAllCoinsAsync();

        var existingExternalIds = (await _cryptoRepository.GetAllAsync()).Select(x => x.ExternalId).ToHashSet();
        var existingSymbols = (await _cryptoRepository.GetAllAsync()).Select(x => x.Symbol.ToLower()).ToHashSet();

        foreach (var dto in externalCoins)
        {
            var symbolLower = dto.Symbol.ToLower();


            if (!existingExternalIds.Contains(dto.Id) && !existingSymbols.Contains(symbolLower))
            {
                var newCrypto = new Crypto(symbolLower, dto.Name, dto.Id);
                await _cryptoRepository.AddAsync(newCrypto);

                existingExternalIds.Add(dto.Id);
                existingSymbols.Add(symbolLower);
            }
        }
    }
}