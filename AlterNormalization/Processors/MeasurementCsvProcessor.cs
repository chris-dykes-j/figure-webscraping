using System.Text.RegularExpressions;

namespace AlterNormalization.Processors;

public class MeasurementCsvProcessor : CsvProcessor
{
    public MeasurementCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessFirstLine() => "name,measurement\n";

    public override string ProcessLine(string line)
    {
        var columns = SplitIgnoringQuotes(line, ',');
        var measurements = SplitFigureMeasurements(columns[5]);
        var result = "";
        foreach (var measurement in measurements)
        {
            result += $"{columns[0]},{measurement}\n";
        }

        return result;
    }
    
    private List<string> SplitFigureMeasurements(string measurement)
    {
        measurement = Regex.Replace(measurement, @"1/[1-9] スケール ", "");
        
        /* AI funny business. Let me handle this.
        var sizePattern = @"[^,]+：[^,\s]+";
        var matches = Regex.Matches(measurement, sizePattern);
        var sizes = new List<string>();

        foreach (Match match in matches)
        {
            sizes.Add(match.Value.Trim());
        }
        */
        return new() { measurement }; // For now
    }
}