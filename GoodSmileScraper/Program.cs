using ScraperTools;

const string csvPath = "/home/chris/RiderProjects/FigureWebScraper/GoodSmileScraper/goodsmile-table.csv";
if (!File.Exists(csvPath))
{
    await using var writer = File.CreateText(csvPath);
    writer.WriteLine("product-name,series,manufacturer,category,price,release,specifications,sculptor,paintwork,planning/production," +
                     "designer,illustrator,released-by,distributed-by,cooperation,director,url");
}

var httpClient = new ScraperClient().HttpClient;

Console.WriteLine("Scraping complete!");

