using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

Console.Write("Input year: ");
var searchYear = Console.ReadLine();
Console.Write("Input section: ");
var section = Console.ReadLine();

const string csvPath = "/home/chris/RiderProjects/FigureWebScraper/AlterScraper/alter.csv";
if (!File.Exists(csvPath))
{
    await using var writer = File.CreateText(csvPath);
    writer.WriteLine("name,series,character,release,price,scale,size,sculptor,painter,material,brand,img_directory");
}

var httpClient = new HttpClient();
var mozilla = new ProductInfoHeaderValue("Mozilla", "5.0");
var system = new ProductInfoHeaderValue("(X11; Linux x86_64; rv:108.0)"); 
var gecko = new ProductInfoHeaderValue("Gecko", "20100101");
var firefox = new ProductInfoHeaderValue("FireFox", "108.0");

httpClient.DefaultRequestHeaders.UserAgent.Add(mozilla);
httpClient.DefaultRequestHeaders.UserAgent.Add(system);
httpClient.DefaultRequestHeaders.UserAgent.Add(gecko);
httpClient.DefaultRequestHeaders.UserAgent.Add(firefox);

var requestUri = $"https://alter-web.jp/{section}/?yy={searchYear}&mm=";
using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

var response = await httpClient.SendAsync(request).Result.Content.ReadAsStringAsync();
var homePage = new HtmlDocument();
homePage.LoadHtml(response);

await using (var streamWriter = File.AppendText(csvPath))
{
    foreach (var link in homePage.DocumentNode.SelectNodes("//figure/a"))
    {
        Thread.Sleep(2000);
        using var figureRequest = new HttpRequestMessage(HttpMethod.Get, "https://alter-web.jp" + link.Attributes["href"].Value);
        figureRequest.Headers.Referrer = new Uri(requestUri);
        var figureResponse = await httpClient.SendAsync(figureRequest).Result.Content.ReadAsStringAsync();
        var figurePage = new HtmlDocument();
        figurePage.LoadHtml(figureResponse);
        var result = ParseFigurePage(figurePage);
        streamWriter.WriteLine(result);
    }
}

string ParseFigurePage(HtmlDocument figurePage)
{
    var name = figurePage.DocumentNode.SelectSingleNode($"//h1[@class='hl06 c-{section}']");
    var tableItems = figurePage.DocumentNode.SelectNodes("//td");
    var material = figurePage.DocumentNode.SelectSingleNode("//span[@class='txt']");
    // name,series,character,release,price,scale,size,sculptor,painter,material,brand,img_directory

    var result = $"\"{name.InnerText}\","; 
    foreach (var item in tableItems)
    {
        result += $"\"{item.InnerHtml}\",".Replace("<br>", " ");
    }

    result += material.InnerText; //[..material.InnerText.IndexOf('<')] ?? material.InnerText;

    return Regex.Replace(result, @"\p{Cc}", ""); //result; // .Replace("\r", "").Replace("\t", "").Replace("\n", "");
}
