using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

Console.Write("Input first year: ");
var searchYear = Console.ReadLine();
Console.Write("Input last year: ");
var finalYear = Console.ReadLine();
Console.Write("Input section: ");
var section = Console.ReadLine();

const string csvPath = "/home/chris/RiderProjects/FigureWebScraper/AlterScraper/alter-jp.csv";
if (!File.Exists(csvPath))
{
    await using var writer = File.CreateText(csvPath);
    writer.WriteLine("name,series,character,release,price,size,sculptor,painter,material,brand,url,blog_url");
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

for (var year = int.Parse(searchYear!); year >= int.Parse(finalYear!); year--)
{
    await GetFigureData(year);
    Thread.Sleep(15000);
}

async Task GetFigureData(int year)
{
    var requestUri = $"https://alter-web.jp/{section}/?yy={year}&mm=";
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
        var result = await ParseFigurePage(figurePage, figureLink, year);
        File.AppendAllText(csvPath, result + "\n");
        Console.WriteLine(result);
    }
}

Console.WriteLine("Scraping complete!");

async Task<string> ParseFigurePage(HtmlDocument figurePage, string figureLink, int year)
{
    var name = figurePage.DocumentNode.SelectSingleNode($"//h1[@class='hl06 c-{section}']");
    var tableItems = figurePage.DocumentNode.SelectNodes("//td");
    var material = figurePage.DocumentNode.SelectSingleNode("//span[@class='txt']");
    var blogLinks = figurePage.DocumentNode.SelectNodes("//div[@class='imgtxt imgtxt-type-b img-l']//a");
    const string brand = "Alter";
    
    var result = $"\"{name.InnerText}\","; 
    foreach (var item in tableItems)
    {
        result += $"\"{item.InnerHtml}\",".Replace("<br>", " ")
            .Replace($"<span class=\"resale c-{section}\">再販</span>", " ");
    }
    result += $"\"{material.InnerText}\",{brand},{figureLink},";

    result += "\"";
    if (blogLinks != null)
    {
        foreach (var blogLink in blogLinks)
        {
            result += $"{blogLink.Attributes["href"].Value},";
        }
    }
    result += "\"";
    
    await GetFigureImages(figurePage, name.InnerText, figureLink, year);
    
    return Regex.Replace(result, @"\p{Cc}", "");
}

async Task GetFigureImages(HtmlDocument figurePage, string name, string referrer, int year)
{
    var path = $"/run/media/chris/Extra/Figures/Alter/{year}/{name}";
    if (!Directory.Exists(path))
    {
        Directory.CreateDirectory(path);
    }
    else
    {
        return;
    }
    
    var firstImage = figurePage.DocumentNode.SelectSingleNode("//div[@class='item-mainimg']//img");
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

    var iconLinks = figurePage.DocumentNode.SelectNodes("//div[@class='imgtxt imgtxt-type-b img-l']//img");
    if (iconLinks != null)
    {
        foreach (var iconLink in iconLinks)
        {
            var iconPath = iconLink.Attributes["src"].Value;
            using var iconRequest = new HttpRequestMessage(HttpMethod.Get, $"https://alter-web.jp{iconPath}");
            iconRequest.Headers.Referrer = new Uri(referrer);
            await using var streamToRead = await httpClient.SendAsync(iconRequest).Result.Content.ReadAsStreamAsync();
            await using var streamToWrite = File.Open($"{path}/{iconPath[20..]}", FileMode.Create);
            await streamToRead.CopyToAsync(streamToWrite);
        }
    }
}