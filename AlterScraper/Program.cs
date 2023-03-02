using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

Console.Write("Input year: ");
var searchYear = Console.ReadLine();
Console.Write("Input section: ");
var section = Console.ReadLine();
const string brand = "Alter";

const string csvPath = "/home/chris/RiderProjects/FigureWebScraper/AlterScraper/alter-jp.csv";
if (!File.Exists(csvPath))
{
    await using var writer = File.CreateText(csvPath);
    writer.WriteLine("name,series,character,release,price,size,sculptor,painter,material,brand");
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

var random = new Random();

foreach (var link in homePage.DocumentNode.SelectNodes("//figure/a"))
{
    Thread.Sleep(random.Next(1000, 3000));
    var figureLink = "https://alter-web.jp" + link.Attributes["href"].Value;
    using var figureRequest = new HttpRequestMessage(HttpMethod.Get, figureLink);
    figureRequest.Headers.Referrer = new Uri(requestUri);
    var figureResponse = await httpClient.SendAsync(figureRequest).Result.Content.ReadAsStringAsync();
    var figurePage = new HtmlDocument();
    figurePage.LoadHtml(figureResponse);
    var result = await ParseFigurePage(figurePage, figureLink);
    File.AppendAllText(csvPath, result + "\n");
    Console.WriteLine(result);
}

async Task<string> ParseFigurePage(HtmlDocument figurePage, string figureLink)
{
    var name = figurePage.DocumentNode.SelectSingleNode($"//h1[@class='hl06 c-{section}']");
    var tableItems = figurePage.DocumentNode.SelectNodes("//td");
    var material = figurePage.DocumentNode.SelectSingleNode("//span[@class='txt']");
    
    var result = $"\"{name.InnerText}\","; 
    foreach (var item in tableItems)
    {
        result += $"\"{item.InnerHtml}\",".Replace("<br>", " ");
    }
    result += $"\"{material.InnerText}\",{brand}";

    await GetFigureImages(figurePage, name.InnerText, figureLink);
    
    return Regex.Replace(result, @"\p{Cc}", "");
}

async Task GetFigureImages(HtmlDocument figurePage, string name, string referrer)
{
    var path = $"/run/media/chris/Extra/Figures/Alter/{searchYear}/{name}";
    if (!Directory.Exists(path))
    {
        Directory.CreateDirectory(path);
    }
    
    var firstImage = figurePage.DocumentNode.SelectSingleNode("//div[@class='item-mainimg']//img");// .FirstChild("//img");
    var firstImagePath = firstImage.Attributes["src"].Value;
    using var figureRequest = new HttpRequestMessage(HttpMethod.Get, $"https://alter-web.jp/{firstImagePath}");
    figureRequest.Headers.Referrer = new Uri(referrer);
    await using (var streamToRead = await httpClient.SendAsync(figureRequest).Result.Content.ReadAsStreamAsync()) 
    {
        await using (var streamToWrite = File.Open($"{path}/{firstImagePath[17..]}", FileMode.Create))
        {
            await streamToRead.CopyToAsync(streamToWrite);
        }
    }

    var imageLinks = figurePage.DocumentNode.SelectNodes("//div[@class='imgset']//img");
    foreach (var imageLink in imageLinks)
    {
        var imagePath = imageLink.Attributes["src"].Value;
        using var imageRequest = new HttpRequestMessage(HttpMethod.Get, $"https://alter-web.jp/{imagePath}");
        imageRequest.Headers.Referrer = new Uri(referrer);
        await using var streamToRead = await httpClient.SendAsync(imageRequest).Result.Content.ReadAsStreamAsync();
        await using var streamToWrite = File.Open($"{path}/{imagePath[17..]}", FileMode.Create);
        await streamToRead.CopyToAsync(streamToWrite);
    }
}
