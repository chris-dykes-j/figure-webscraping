namespace AlterNormalization.Processors;

public class ReleaseCsvProcessor : CsvProcessor
{
    public ReleaseCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessFirstLine() => "name,year,month\n";

    public override string ProcessLine(string line)
    {
        var columns = SplitIgnoringQuotes(line, ',');
        var dates = columns[3].TrimEnd().Split(' ');
        var result = "";
        foreach (var date in dates)
        {
            if (!string.IsNullOrWhiteSpace(date))
            {
                result += $"{columns[0]},{date[0..4]},{date[5..7].Trim('月')}\n";
            }
        }
        return result;
    }
}