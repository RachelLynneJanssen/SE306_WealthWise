using Microsoft.AspNetCore.Mvc;
using WealthWise_RCD.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using MySqlX.XDevAPI;
using HtmlAgilityPack;

namespace WealthWise_RCD.Controllers
{
    public class NewsController : Controller
    {
        private readonly HttpClient _httpClient;

        public NewsController()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> Index(bool showAll = false, string stockType = "sp500", string activeTab = "articles")
        {
            var articles = await LoadArticlesAsync();
            var stocks = await LoadStocksAsync(stockType);
            var displayStocks = showAll ? stocks : stocks.Take(10).ToList();

            ViewData["Title"] = "News";
            ViewData["Articles"] = articles;
            ViewData["Stocks"] = displayStocks;
            ViewData["ShowAll"] = showAll;
            ViewData["ActiveTab"] = activeTab;
            ViewData["StockType"] = stockType;

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

        private async Task<List<Stock>> LoadStocksAsync(string stockType)
        {
            string url = stockType switch
            {
                "sp500" => "https://stockanalysis.com/list/sp-500-stocks/",
                "nasdaq" => "https://stockanalysis.com/list/nasdaq-100-stocks/",
                _ => "https://stockanalysis.com/list/sp-500-stocks/"
            };

            var stocks = new List<Stock>();

            HttpClient client = new HttpClient();
            string html = await client.GetStringAsync(url);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var table = doc.DocumentNode.SelectSingleNode("//table");

            if (table != null)
            {
                var rows = table.SelectNodes(".//tr");

                for (int i = 1; i < rows.Count; i++)
                {
                    var cells = rows[i].SelectNodes(".//td");
                    if (cells != null && cells.Count >= 3)
                    {
                        string ticker = cells[0].InnerText.Trim();
                        string name = cells[1].InnerText.Trim();
                        string sector = cells[2].InnerText.Trim();
                        string marketCap = cells[3].InnerText.Trim();
                        string price = cells[4].InnerText.Trim();
                        string percentChange = cells[5].InnerText.Trim();
                        string revenue = cells[6].InnerText.Trim();

                        stocks.Add(new Stock { Ticker = ticker, Name = name, Sector = sector, MarketCap = marketCap, Price = price, PercentChange = percentChange, Revenue = revenue });
                    }
                }
            }
            else
            {
                stocks.Add(new Stock { Ticker = "Error", Name = "Error", Sector = "Error", MarketCap = "Error", Price = "Error", PercentChange = "Error", Revenue = "Error" });
            }
            return stocks;           
        }
    }
}
