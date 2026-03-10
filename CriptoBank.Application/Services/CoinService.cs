

using System.Net.Http.Json;
using System.Text.Json;
using CriptoBank.Application.DTOs.Crypto;
using CriptoBank.Application.Interfaces.CoinService;
using CriptoBank.Domain.Models;
using CriptoBank.Domain.Repositories;

namespace CriptoBank.Application.Services;

public class CoinService : ICoinService
{
    private readonly HttpClient _http;
    private readonly ICryptoRepository _cryptoRepository;

    public CoinService(HttpClient http, ICryptoRepository cryptoRepository)
    {
        _cryptoRepository = cryptoRepository;
        _http = http;

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

    public async Task<List<CoinMarketDto?>> GetCoinDataAsync(string externalId)
    {
        var url = $"https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&ids={externalId}";

        var response = await _http.GetAsync(url);

        if (!response.IsSuccessStatusCode) return new List<CoinMarketDto>();

        var content = await response.Content.ReadAsStringAsync();

        var coins = JsonSerializer.Deserialize<List<CoinMarketDto>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return coins ?? new List<CoinMarketDto>(); 
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