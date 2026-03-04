using System.Text.Json.Serialization;

namespace CriptoBank.Application.DTOs.Crypto;

public class CoinMarketDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("symbol")]
    public string Symbol { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("current_price")] 
    public decimal Current_Price { get; set; }

    [JsonPropertyName("market_cap")]
    public decimal Market_Cap { get; set; }

    [JsonPropertyName("price_change_percentage_1h_in_currency")]
    public decimal? Price_Change_Percentage_1h_in_Currency { get; set; }
}