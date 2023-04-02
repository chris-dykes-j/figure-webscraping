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
            result += $"{columns[0]},{measurement.Trim()}\n";
        }
        
        return result;
    }
    
    private string[] SplitFigureMeasurements(string measurement)
    {
        measurement = Regex.Replace(measurement, @"1/\d+ スケール ", "");
        return measurement.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    }
}