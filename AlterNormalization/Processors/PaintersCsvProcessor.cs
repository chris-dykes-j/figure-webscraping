using System.Text.RegularExpressions;

namespace AlterNormalization.Processors;

public class PaintersCsvProcessor : CsvProcessor
{
    public PaintersCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessFirstLine() => "name,painter\n";

    public override string ProcessLine(string line)
    {
        var columns = SplitIgnoringQuotes(line, ',');
        var splitPaintersFirst = SplitSculptorsByBracket(Regex.Split(columns[6], @"[】）]\s"));
        var splitPaintersSecond = SplitPaintersByPlus( splitPaintersFirst);

        var result = "";
        foreach (var painter in splitPaintersSecond)
        {
            result += $"{columns[0]},{painter}\n";
        }

        return result;
    }
    
    private List<string> SplitSculptorsByBracket(string[] painters)
    {
        var result = new List<string>();
        foreach (var painter in painters)
        {
            var sculptorResult = painter;
            if (string.IsNullOrWhiteSpace(painter)) continue;
            if (painter.Contains("原型協力：アルター")) continue;
            if (MissingSquareBracket(painter)) sculptorResult += '】';
            if (MissingParenthesis(painter)) sculptorResult += '）';
            result.Add(sculptorResult);
        }

        return result;
    }

    private List<string> SplitPaintersByPlus(List<string> painters) => 
        painters.SelectMany(sculptor => sculptor.Split('＋', StringSplitOptions.RemoveEmptyEntries)).ToList();

    private bool MissingSquareBracket(string input) => input.Contains('【') && !input.Contains('】');
    
    private bool MissingParenthesis(string input) => input.Contains('（') && !input.Contains('）');
}