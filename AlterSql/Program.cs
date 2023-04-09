
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
    return $"WITH inserted_figure AS (\n" +
           $"  INSERT INTO figure (scale, brand, origin_url) VALUES ('{columns[2]}', '{columns[3]}', '{columns[4]}')\n" +
           $"  RETURNING id\n" +
           $"),\n" +
           $"inserted_figure_name AS (\n" +
           $"  INSERT INTO figure_name (figure_id, language_code, text)\n" +
           $"  SELECT id, 'ja', '{columns[0]}' FROM inserted_figure\n" +
           $"  RETURNING figure_id\n" +
           $")\n" +
           $"INSERT INTO series_name (figure_id, language_code, text)\n" +
           $"SELECT figure_id, 'ja', '{columns[1]}' FROM inserted_figure_name;\n";


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
