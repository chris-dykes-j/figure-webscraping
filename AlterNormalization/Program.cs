using System.Text.RegularExpressions;

const string readPath = "/home/chris/RiderProjects/FigureWebScraper/AlterScraper/alter-jp.csv";

string CsvPath(string tableName) => 
    $"/home/chris/RiderProjects/FigureWebScraper/AlterNormalization/alter-{tableName}-jp.csv";

using var streamWriter = File.CreateText(CsvPath("figures"));
using var streamReader = new StreamReader(readPath);

Console.WriteLine("Starting normalization");
while (!streamReader.EndOfStream)
{
    var line = streamReader.ReadLine();
    var columns = SplitIgnoringQuotes(line!, ',');
    var scaleRegex = new Regex(@"^1\/[1-9]");
    var scale = scaleRegex.Match(columns[5]).ToString();
    if (string.IsNullOrEmpty(scale)) scale = "scale";
    //var size = scaleRegex.Replace(columns[5], "").Replace("スケール ", "");
    var figuresValues = $"{columns[0]},{columns[1]},{columns[2]},{scale},{columns[9]},{columns[10]}";
    File.AppendAllText(CsvPath("figures"), figuresValues + '\n');
    Console.WriteLine(figuresValues);
}

// Thx chat-gpt
List<string> SplitIgnoringQuotes(string line, char delimiter)
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
// Handle repeating groups in release, price, sculptor, painter, material, blog_url

// Separate year and month.