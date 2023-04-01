using AlterNormalization.Processors;

const string readPath = "/home/chris/RiderProjects/FigureWebScraper/AlterScraper/alter-jp.csv";

var csvProcessors = new List<CsvProcessor> // More to be added.
{
    new FiguresCsvProcessor("figures"),
    new ReleaseCsvProcessor("release-dates"),
    new PriceCsvProcessor("prices")
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
        File.AppendAllText(csvProcessor.OutputPath, outputLine);
        //Console.WriteLine(outputLine);
    }
    isFirstLine = false;
}
Console.WriteLine("Finished!");

// var size = scaleRegex.Replace(columns[5], "").Replace("スケール", "");