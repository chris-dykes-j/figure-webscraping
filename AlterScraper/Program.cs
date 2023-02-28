using System.Net.Http.Headers;
using HtmlAgilityPack;

// Input which year/years to scrape.
const string searchYear = "2023";
const string section = "figure";

// Create new csv file.


// Create header to mimic regular user. Great way to learn about request headers.
var httpClient = new HttpClient();
var mozilla = new ProductInfoHeaderValue("Mozilla", "5.0");
var system = new ProductInfoHeaderValue("(X11; Linux x86_64; rv:108.0)"); 
var gecko = new ProductInfoHeaderValue("Gecko", "20100101");
var firefox = new ProductInfoHeaderValue("FireFox", "108.0");

httpClient.DefaultRequestHeaders.UserAgent.Add(mozilla);
httpClient.DefaultRequestHeaders.UserAgent.Add(system);
httpClient.DefaultRequestHeaders.UserAgent.Add(gecko);
httpClient.DefaultRequestHeaders.UserAgent.Add(firefox);

const string requestUri = $"https://alter-web.jp/{section}/?yy={searchYear}&mm=";
var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

var response = await httpClient.SendAsync(request).Result.Content.ReadAsStringAsync();
var homePage = new HtmlDocument();
homePage.LoadHtml(response);
Thread.Sleep(5000);

foreach (var link in homePage.DocumentNode.SelectNodes("//figure/a"))
{
    Thread.Sleep(2000);
    var figureRequest = new HttpRequestMessage(HttpMethod.Get, "https://alter-web.jp" + link.Attributes["href"].Value);
    figureRequest.Headers.Referrer = new Uri(requestUri);
    var figureResponse = await httpClient.SendAsync(figureRequest).Result.Content.ReadAsStringAsync();
    var figurePage = new HtmlDocument();
    figurePage.LoadHtml(figureResponse);
}

// ?

// Profit.
