const string readPath = "/home/chris/RiderProjects/FigureWebScraper/AlterNormalization/Output/alter-figures-jp.csv";
const string outputPath = "/home/chris/RiderProjects/FigureWebScraper/AlterSql/Sql/insert.sql";

File.Create(outputPath).Dispose();
using var streamReader = new StreamReader(readPath);

streamReader.ReadLine(); // Skip first line.
while (!streamReader.EndOfStream)
{
    var outputLine = ProcessLine(streamReader.ReadLine()!);
    File.AppendAllText(outputPath, outputLine);
}

string ProcessLine(string line)
{
    var columns = SplitIgnoringQuotes(line, ',');
    return 
        "INSERT INTO figure (id, scale, brand, origin_url) " +
        $"VALUES ('{columns[0]}', '{columns[3]}', '{columns[4]}', '{columns[5]}');\n" +
        "INSERT INTO figure_name (figure_id, language_code, text) " +
        $"VALUES ('{columns[0]}', 'ja', '{columns[1]}');\n" +
        "INSERT INTO series_name (figure_id, language_code, text) " +
        $"VALUES ('{columns[0]}', 'ja', '{columns[2]}');\n";
}

List<string> SplitIgnoringQuotes(string line, char delimiter)
{
    var result = new List<string>();
    var inQuotes = false;
    var start = 0;

    for (var i = 0; i < line.Length; i++)
    {
        if (line[i] == '"') inQuotes = !inQuotes;
        if (line[i] != delimiter || inQuotes) continue;
        result.Add(line
            .Substring(start, i - start)
            .Replace("'", "''")
            .Trim('\"', ' '));
        start = i + 1;
    }

    result.Add(line[start..].Trim('\"', ' '));

    return result;
}
