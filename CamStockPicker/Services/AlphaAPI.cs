using System.Diagnostics;
using System.Net.Http.Json;
using CamStockPicker.Models;

namespace CamStockPicker.Services
{
    /// <summary>
    /// Responsible for communicating with the Alpha Vantage API.
    /// </summary>
    public class AlphaAPI
    {
        private readonly HttpClient _httpClient;

        private const string ApiKey = "XQF4IWBQO4QEOYCT";
        private const string BaseUrl = "https://www.alphavantage.co/query";

        public AlphaAPI()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Executes a search to find stocks.
        /// </summary>
        public async Task<List<StockSearchResult>> SearchStocksAsync(string keyword)
        {
            try
            {
                string url =
                    $"{BaseUrl}?function=SYMBOL_SEARCH&keywords={Uri.EscapeDataString(keyword)}&apikey={ApiKey}";

                var response = await _httpClient.GetFromJsonAsync<StockSearchResponse>(url);
                return response?.BestMatches ?? new List<StockSearchResult>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AlphaAPI] Search Error: {ex.Message}");
                return new List<StockSearchResult>();
            }
        }

        /// <summary>
        /// Executes a quote to get the latest data.
        /// </summary>
        public async Task<StockQuote?> GetQuoteAsync(string symbol)
        {
            try
            {
                string url =
                    $"{BaseUrl}?function=GLOBAL_QUOTE&symbol={Uri.EscapeDataString(symbol)}&apikey={ApiKey}";

                var response = await _httpClient.GetFromJsonAsync<GlobalQuoteResponse>(url);
                return response?.Quote;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AlphaAPI] Quote Error: {ex.Message}");
                return null;
            }
        }
    }
}