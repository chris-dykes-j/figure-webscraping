namespace AlterNormalization.Processors;

public class BlogUrlsCsvProcessor : CsvProcessor
{
    public BlogUrlsCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessFirstLine() => "figure_id,blog_url\n";

    public override string ProcessLine(string line)
    {
        var columns = SplitIgnoringQuotes(line, ',');
        var blogUrls = SplitBlogUrls(columns[11]);
        return blogUrls.Aggregate("", (current, blogUrl) => current + $"{columns[12]},https://alter-web.jp{blogUrl}\n");
    }
    
    private string[] SplitBlogUrls(string blogUrls) => blogUrls.Split(',', StringSplitOptions.RemoveEmptyEntries);
}