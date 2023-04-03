namespace AlterNormalization.Processors;

public class ReleaseDatesCsvProcessor : CsvProcessor
{
    public ReleaseDatesCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessFirstLine() => "name,year,month\n";

    public override string ProcessLine(string line)
    {
        var columns = SplitIgnoringQuotes(line, ',');
        var dates = columns[3].TrimEnd().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return dates.Aggregate("", (current, date) => current + $"{columns[0]},{date[0..4]},{date[5..7].Trim('月')}\n");
    }
}