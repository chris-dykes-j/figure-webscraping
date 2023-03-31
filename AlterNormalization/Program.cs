using AlterNormalization.Processors;

const string readPath = "/home/chris/RiderProjects/FigureWebScraper/AlterScraper/alter-jp.csv";

string WritePath(string tableName) => $"/home/chris/RiderProjects/FigureWebScraper/AlterNormalization/alter-{tableName}-jp.csv";

var csvProcessors = new List<CsvProcessor> // More to be added.
{
    new FiguresCsvProcessor(WritePath("figures"))
};

Console.WriteLine("Starting normalization");
using var streamReader = new StreamReader(readPath);
var isFirstLine = true;
while (!streamReader.EndOfStream)
{
    var line = streamReader.ReadLine();
    foreach (var csvProcessor in csvProcessors) // may want to move internal logic to CsvProcessor classes.
    {
        var outputLine = isFirstLine ? csvProcessor.ProcessFirstLine() : csvProcessor.ProcessLine(line!);
        File.AppendAllText(csvProcessor.OutputPath, outputLine + '\n');
        Console.WriteLine(outputLine);
    }
    isFirstLine = false;
}

// var size = scaleRegex.Replace(columns[5], "").Replace("スケール", "");