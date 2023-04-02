using System.Collections;
using System.Text.RegularExpressions;

namespace AlterNormalization.Processors;

public class SculptorCsvProcessor : CsvProcessor
{
    public SculptorCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessFirstLine() => "name,sculptor\n";

    public override string ProcessLine(string line)
    {
        var columns = SplitIgnoringQuotes(line, ',');
        var splitSculptorsFirst = SplitSculptorsByBracket(Regex.Split(columns[6], @"[】）]\s"));
        var splitSculptorsSecond = SplitSculptorsByPlus( splitSculptorsFirst);

        var result = "";
        foreach (var sculptor in splitSculptorsSecond)
        {
            result += $"{columns[0]},{sculptor}\n";
        }

        return result;
    }
    
    private List<string> SplitSculptorsByBracket(string[] sculptors)
    {
        var result = new List<string>();
        foreach (var sculptor in sculptors)
        {
            var sculptorResult = sculptor;
            if (string.IsNullOrWhiteSpace(sculptor)) continue;
            if (sculptor.Contains("原型協力：アルター")) continue;
            if (sculptor.Contains('【') && !sculptor.Contains('】')) sculptorResult += '】';
            if (sculptor.Contains('（') && !sculptor.Contains('）')) sculptorResult += '）';
            result.Add(sculptorResult);
        }

        return result;
    }

    private List<string> SplitSculptorsByPlus(List<string> sculptors) => 
        sculptors.SelectMany(sculptor => sculptor.Split('＋', StringSplitOptions.RemoveEmptyEntries)).ToList();
}