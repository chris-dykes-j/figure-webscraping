using System.Text.RegularExpressions;

namespace AlterNormalization.Processors;

public class MeasurementsCsvProcessor : CsvProcessor
{
    public MeasurementsCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessFirstLine() => "name,measurement\n";

    public override string ProcessLine(string line)
    {
        var columns = SplitIgnoringQuotes(line, ',');
        var measurements = SplitFigureMeasurements(columns[5]);
        var result = "";
        foreach (var measurement in measurements)
        {
            result += measurement switch
            {
                _ when MissingCloseSquareBracket(measurement) => $"{columns[0]},{measurement}",
                _ when MissingOpenSquareBracket(measurement) => $"{measurement.Trim()}\n",
                _ => $"{columns[0]},{measurement.Trim()}\n"
            };
        }
        
        return result;
    }
    
    private string[] SplitFigureMeasurements(string measurement)
    {
        measurement = Regex.Replace(measurement, @"1/\d+ スケール ", "")
            .Replace("NON", "")
            .Replace("スケール", "");
        return measurement.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    }
    
    private bool MissingCloseSquareBracket(string input) => input.Contains('【') && !input.Contains('】');
    private bool MissingOpenSquareBracket(string input) => input.Contains('】') && !input.Contains('【');
}