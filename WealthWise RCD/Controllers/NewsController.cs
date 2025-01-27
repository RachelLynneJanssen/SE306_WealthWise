using Microsoft.AspNetCore.Mvc;
using WealthWise_RCD.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace WealthWise_RCD.Controllers
{
    //hi
    public class NewsController : Controller
    {
        private readonly HttpClient _httpClient;

        public NewsController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {
            var articles = await LoadArticlesAsync();
            var stocks = await LoadStocksAsync();

            ViewData["Title"] = "News";
            ViewData["Articles"] = articles;
            ViewData["Stocks"] = stocks;

            return View();
        }

        private async Task<List<Article>> LoadArticlesAsync()
        {
            var articles = new List<Article>();
            HttpResponseMessage response = await _httpClient.GetAsync("https://newsdata.io/api/1/latest?apikey=pub_619842b8858aa6f8c351442dd9b4c8af069f3&q=financial");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var jsonObject = JsonDocument.Parse(jsonString).RootElement;

                if (jsonObject.TryGetProperty("results", out JsonElement resultsElement))
                {
                    foreach (var result in resultsElement.EnumerateArray())
                    {
                        string pageTitle = result.TryGetProperty("title", out var titleElement) ? titleElement.GetString() : "No data available";
                        string creator = result.TryGetProperty("creator", out var creatorElement) && creatorElement.ValueKind == JsonValueKind.Array
                            ? creatorElement[0].GetString()
                            : "No data available";
                        string link = result.TryGetProperty("link", out var linkElement) ? linkElement.GetString() : "No data available";
                        string pubDate = result.TryGetProperty("pubDate", out var pubDateElement) ? pubDateElement.GetString() : "No data available";

                        articles.Add(new Article { Title = pageTitle, Author = creator, Link = link, PublishDate = pubDate });
                    }
                }
            }

            return articles;
        }

        private async Task<List<string>> LoadStocksAsync()
        {
            var stocks = new List<string>();
            var stockSymbols = new List<string> { "AAPL", "MSFT", "GOOGL", "AMZN", "TSLA", "META", "NVDA", "BRK-B", "JNJ", "V" };

            foreach (var symbol in stockSymbols)
            {
                var response = await _httpClient.GetAsync($"https://api.polygon.io/v2/aggs/ticker/{symbol}/prev?apiKey=69raDZohS9ejuVi3eV25ppkuYKFl7FuH");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var jsonObject = JsonDocument.Parse(jsonString).RootElement;

                    if (jsonObject.TryGetProperty("results", out JsonElement resultsElement) && resultsElement.GetArrayLength() > 0)
                    {
                        var stockData = resultsElement[0]; // Get the first result
                        decimal price = stockData.GetProperty("c").GetDecimal(); // Close price
                        stocks.Add($"{symbol}: ${price:F2}");
                    }
                }
                else
                {
                    stocks.Add($"{symbol}: Data not available");
                }
            }

            return stocks;
        }
    }
}
