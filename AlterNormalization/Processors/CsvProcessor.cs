namespace AlterNormalization.Processors;

public abstract class CsvProcessor
{
    public string OutputPath { get; }
    
    protected CsvProcessor(string outPath)
    {
        OutputPath = WritePath(outPath);
        CreateOutputFile(); // Breaks single responsibility but w/e.
    }

    private string WritePath(string tableName) => 
        $"/home/chris/RiderProjects/FigureWebScraper/AlterNormalization/Output/alter-{tableName}-jp.csv";
    
    private void CreateOutputFile()
    {
        using var streamWriter = File.Create(OutputPath);
    }

    protected List<string> SplitIgnoringQuotes(string line, char delimiter)
    {
        var result = new List<string>();
        var inQuotes = false;
        var start = 0;

        for (var i = 0; i < line.Length; i++)
        {
            if (line[i] == '"') inQuotes = !inQuotes;

            if (line[i] == delimiter && !inQuotes)
            {
                result.Add(line.Substring(start, i - start).Trim('\"', ' '));
                start = i + 1;
            }
        }
        result.Add(line[start..].Trim('\"', ' '));

        return result;
    }
    
    public abstract string ProcessFirstLine();
    public abstract string ProcessLine(string line);
}