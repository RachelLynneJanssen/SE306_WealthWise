namespace WealthWise_RCD.Models
{
    public class FinanceResource
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }

    public static class FinanceResourceData
    {
        public static List<FinanceResource> GetResources()
        {
            return new List<FinanceResource>
        {
            new FinanceResource { Name = "Bloomberg", Url = "https://www.bloomberg.com", Description = "Comprehensive business and markets coverage, including real-time data, analysis, and video from Bloomberg News and Businessweek." },
            new FinanceResource { Name = "Reuters – Finance", Url = "https://www.reuters.com/business/finance/", Description = "Global finance headlines, breaking news, and in-depth reporting on markets, economies, and corporate developments." },
            new FinanceResource { Name = "CNBC – Finance", Url = "https://www.cnbc.com/finance/", Description = "Live market news, stock quotes, personal finance advice, and video content from the U.S. business-news network." },
            new FinanceResource { Name = "MarketWatch", Url = "https://www.marketwatch.com/", Description = "Stock market data, business news, analysis, and personal-finance tools powered by Dow Jones." },
            new FinanceResource { Name = "Financial Times – Markets", Url = "https://www.ft.com/markets", Description = "Award-winning international finance reporting, with a focus on markets, economics, and global business." },
            new FinanceResource { Name = "The Wall Street Journal – Markets", Url = "https://www.wsj.com/news/markets", Description = "Authoritative coverage of U.S. and global financial news, markets, and economic policy." },
            new FinanceResource { Name = "Barron’s", Url = "https://www.barrons.com/", Description = "Deep-dive analysis on stocks, investing strategies, and market trends from the Financial Times Group." },
            new FinanceResource { Name = "Forbes – Finance", Url = "https://www.forbes.com/finance/", Description = "Original articles on investing, personal finance, wealth management, and market news." },
            new FinanceResource { Name = "Business Insider – Finance", Url = "https://www.businessinsider.com/finance", Description = "Fast-paced business and market news, with a strong focus on tech-driven financial trends." },
            new FinanceResource { Name = "Yahoo Finance", Url = "https://finance.yahoo.com/", Description = "Free stock quotes, portfolio tracking, and up-to-the-minute finance news." },
            new FinanceResource { Name = "The Economist – Finance & Economics", Url = "https://www.economist.com/finance-and-economics", Description = "In-depth analysis of global economic trends, markets, and policy from a renowned weekly." },
            new FinanceResource { Name = "TheStreet", Url = "https://www.thestreet.com/", Description = "Market news, investing ideas, and personal finance advice from Jim Cramer’s team." },
            new FinanceResource { Name = "Seeking Alpha", Url = "https://seekingalpha.com/", Description = "Crowdsourced market analysis, earnings transcripts, and opinion pieces on stocks and ETFs." },
            new FinanceResource { Name = "Investopedia – News", Url = "https://www.investopedia.com/news/", Description = "Finance news alongside educational explainers, market summaries, and investment tutorials." },
            new FinanceResource { Name = "Kiplinger", Url = "https://www.kiplinger.com/finance", Description = "Personal finance, retirement planning, and investing advice paired with market commentary." },
            new FinanceResource { Name = "Morningstar – News", Url = "https://www.morningstar.com/news", Description = "Independent mutual-fund and ETF analysis alongside broader market news and data." },
            new FinanceResource { Name = "Money (Time magazine)", Url = "https://money.com/", Description = "Personal-finance reporting, consumer advice, and wealth-building strategies." },
            new FinanceResource { Name = "Finimize", Url = "https://www.finimize.com/", Description = "Concise daily finance news and market briefs designed for busy readers." },
            new FinanceResource { Name = "Financial News (FN London)", Url = "https://www.fnlondon.com/", Description = "Europe-focused coverage of banking, asset management, and markets for finance professionals." },
            new FinanceResource { Name = "Moneycontrol", Url = "https://www.moneycontrol.com/news/business", Description = "Leading Indian portal for finance news, market data, and personal-finance guidance." }        };
        }
    }

}
