// Input which year/years to scrape.

// Create new csv file.

// Create header to mimic regular user. Great way to learn about request headers.

using System.Net.Http.Headers;

var httpClient = new HttpClient();
var userAgent = new ProductInfoHeaderValue("Mozilla/5.0 (X11; Linux x86_64; rv:108.0) Gecko/20100101 Firefox/108.0");
httpClient.DefaultRequestHeaders.UserAgent.Add(userAgent);

var request = new HttpRequestMessage(HttpMethod.Get, "https:www.google.ca");
var response = await httpClient.SendAsync(request);

var test = response;

// Make list of links. <a> tags with /products/ href.

// Make timed requests to each link, and get data.

// Add each line to a csv file.

// Save images to a separate directories?

// ?

// Profit.

Console.WriteLine("Profit");

