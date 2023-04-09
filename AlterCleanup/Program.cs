var streamReader = new StreamReader("/home/chris/RiderProjects/FigureWebScraper/AlterScraper/alter-jp.csv");
var headers = streamReader.ReadLine();
using var output = new StreamWriter("/home/chris/RiderProjects/FigureWebScraper/AlterCleanup/alter-jp.csv");
output.WriteLine($"{headers},id");
var list = new List<string>();
var id = 1;
while (!streamReader.EndOfStream)
{
    var line = streamReader.ReadLine();
    if (list.Contains(line!))
    {
        var columns = SplitIgnoringQuotes(line!, ',');
        Console.WriteLine($"Duplicate: {columns[0]}, {columns[3]}");
        continue;
    }
    list.Add(line!);
    output.WriteLine($"{line},{id}");
    id++;
}

List<string> SplitIgnoringQuotes(string line, char delimiter) // Useful to visually validate.
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
            .Trim('\"', ' '));
        start = i + 1;
    }
    result.Add(line[start..].Trim('\"', ' '));

    return result;
}
