using System.Text.RegularExpressions;

namespace AlterNormalization.Processors;

public class FiguresCsvProcessor : CsvProcessor
{
    public FiguresCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessFirstLine() => "id,name,series,scale,brand,origin_url\n";

    public override string ProcessLine(string line)
    {
        var columns = SplitIgnoringQuotes(line, ',');
        var scaleRegex = new Regex(@"^1/\d+");
        var figureScale = scaleRegex.Match(columns[5]).ToString();

        return $"{columns[12]},{columns[0]},{columns[1]},{figureScale},{columns[9]},{columns[10]}\n";
    }
}