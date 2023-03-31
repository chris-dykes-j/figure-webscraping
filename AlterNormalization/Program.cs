using AlterNormalization.Processors;

const string readPath = "/home/chris/RiderProjects/FigureWebScraper/AlterScraper/alter-jp.csv";

string CsvPath(string tableName) => 
    $"/home/chris/RiderProjects/FigureWebScraper/AlterNormalization/alter-{tableName}-jp.csv";

using var streamReader = new StreamReader(readPath);

var csvProcessors = new List<CsvProcessor>
{
    new FiguresCsvProcessor(readPath, CsvPath("figures"))
};

Console.WriteLine("Starting normalization");
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

//var size = scaleRegex.Replace(columns[5], "").Replace("スケール", "");

// Handle repeating groups in release, price, sculptor, painter, material, blog_url

// Separate year and month.