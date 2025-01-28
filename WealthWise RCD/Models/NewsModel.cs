namespace WealthWise_RCD.Models
{
    public class Article
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string PublishDate { get; set; }
        public string Link { get; set; }
    }

    public class Stock
    {
        public string Ticker { get; set; } // The number placing of the stock
        public string Name { get; set; } // abbreviated stock name
        public string Sector { get; set; } // stock actual company
        public string MarketCap { get; set; }
        public string Price { get; set; }
        public string PercentChange { get; set; }
        public string Revenue { get; set; }
    }
}