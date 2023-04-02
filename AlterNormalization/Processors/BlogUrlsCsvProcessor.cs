namespace AlterNormalization.Processors;

public class BlogUrlsCsvProcessor : CsvProcessor
{
    public BlogUrlsCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessFirstLine() => "name,blog_url\n";

    public override string ProcessLine(string line)
    {
        var columns = SplitIgnoringQuotes(line, ',');
        var blogUrls = SplitBlogUrls(columns[11]);
        var result = "";
        foreach (var blogUrl in blogUrls)
        {
            result += $"{columns[0]},https://alter-web.jp{blogUrl}\n";
        }
        return result;
    }
    
    private string[] SplitBlogUrls(string blogUrls) => blogUrls.Split(',', StringSplitOptions.RemoveEmptyEntries);
}