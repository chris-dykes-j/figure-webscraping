using System.Net.Http.Headers;

namespace ScraperTools;

public class ScraperClient
{
    public HttpClient HttpClient { get; } 
    private readonly ProductInfoHeaderValue _mozilla = new("Mozilla", "5.0");
    private readonly ProductInfoHeaderValue _system = new("(X11; Linux x86_64; rv:108.0)");
    private readonly ProductInfoHeaderValue _gecko = new("Gecko", "20100101");
    private readonly ProductInfoHeaderValue _firefox = new("FireFox", "108.0");
    
    public ScraperClient()
    {
        HttpClient = new HttpClient();
        HttpClient.DefaultRequestHeaders.UserAgent.Add(_mozilla);
        HttpClient.DefaultRequestHeaders.UserAgent.Add(_system);
        HttpClient.DefaultRequestHeaders.UserAgent.Add(_gecko);
        HttpClient.DefaultRequestHeaders.UserAgent.Add(_firefox);
    }
}