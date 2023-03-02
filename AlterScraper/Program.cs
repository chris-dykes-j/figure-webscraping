using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

//Console.Write("Input year: ");
var searchYear = "2023";//Console.ReadLine();
//Console.Write("Input section: ");
var section = "figure"; // Console.ReadLine();

const string csvPath = "/home/chris/RiderProjects/FigureWebScraper/AlterScraper/alter-jp.csv";
if (!File.Exists(csvPath))
{
    await using var writer = File.CreateText(csvPath);
    writer.WriteLine("name,series,character,release,price,scale,size,sculptor,painter,material,brand");
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
await using (var streamWriter = File.AppendText(csvPath))
{
    foreach (var link in homePage.DocumentNode.SelectNodes("//figure/a"))
    {
        Thread.Sleep(random.Next(1000, 3000));
        using var figureRequest = new HttpRequestMessage(HttpMethod.Get, "https://alter-web.jp" + link.Attributes["href"].Value);
        figureRequest.Headers.Referrer = new Uri(requestUri);
        var figureResponse = await httpClient.SendAsync(figureRequest).Result.Content.ReadAsStringAsync();
        var figurePage = new HtmlDocument();
        figurePage.LoadHtml(figureResponse);
        var result = await ParseFigurePage(figurePage);
        streamWriter.WriteLine(result);
    }
}

async Task<string> ParseFigurePage(HtmlDocument figurePage)
{
    var name = figurePage.DocumentNode.SelectSingleNode($"//h1[@class='hl06 c-{section}']");
    var tableItems = figurePage.DocumentNode.SelectNodes("//td");
    var material = figurePage.DocumentNode.SelectSingleNode("//span[@class='txt']");
    
    var result = $"\"{name.InnerText}\","; 
    foreach (var item in tableItems)
    {
        result += $"\"{item.InnerHtml}\",".Replace("<br>", " ");
    }
    result += material.InnerText; 

    await GetFigureImages(figurePage, name.InnerText);
    
    return Regex.Replace(result, @"\p{Cc}", "");
}

async Task GetFigureImages(HtmlDocument figurePage, string name)
{
    var path = $"/home/chris/Pictures/Figures/Alter/{name}";
    if (!Directory.Exists(path))
    {
        Directory.CreateDirectory(path);
    }
    
    var firstImage = figurePage.DocumentNode.SelectSingleNode("//div[@class='item-mainimg']//img");// .FirstChild("//img");
    var firstPath = firstImage.Attributes["src"].Value;
    await using (var firstStream = new FileStream($"{path}/{firstPath[17..]}", FileMode.Create))
    {
        var firstResponse = await httpClient.GetAsync("https://alter-web.jp/{firstPath}");
        await firstResponse.Content.CopyToAsync(firstStream);
    }
    
    var imageLinks = figurePage.DocumentNode.SelectNodes("//div[@class='imgset']//img");
    foreach (var imageLink in imageLinks)
    {
        var imagePath = imageLink.Attributes["src"].Value;
        await using var fileStream = new FileStream($"{path}/{imagePath[17..]}", FileMode.Create);
        var imageResponse = await httpClient.GetAsync("https://alter-web.jp/{imagePath}");
        await imageResponse.Content.CopyToAsync(fileStream);
    }
}
