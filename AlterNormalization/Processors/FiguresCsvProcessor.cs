using System.Text.RegularExpressions;

namespace AlterNormalization.Processors;

public class FiguresCsvProcessor : CsvProcessor
{
    public FiguresCsvProcessor(string outputPath) : base(outputPath) { }

    public override string ProcessFirstLine()
    {
        return "name,series,character,scale,brand,origin_url";
    }

    public override string ProcessLine(string line)
    {
        var columns = SplitIgnoringQuotes(line, ',');
        var scaleRegex = new Regex(@"^1\/[1-9]");
        var figureScale = scaleRegex.Match(columns[5]).ToString();

        return $"{columns[0]},{columns[1]},{columns[2]},{figureScale},{columns[9]},{columns[10]}";
    }
}