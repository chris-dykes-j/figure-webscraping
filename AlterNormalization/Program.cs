using AlterNormalization.Processors;

const string readPath = "/home/chris/RiderProjects/FigureWebScraper/AlterScraper/alter-jp.csv";

var csvProcessors = new List<CsvProcessor> 
{
    new FiguresCsvProcessor("figures"),
    new CharactersCsvProcessor("characters"),
    new ReleaseDatesCsvProcessor("release-dates"),
    new PricesCsvProcessor("prices"),
    new BlogUrlsCsvProcessor("blog-urls"),
    new MeasurementsCsvProcessor("measurements"),
    new MaterialsCsvProcessor("materials"),
    new SculptorsCsvProcessor("sculptors"),
    new PaintersCsvProcessor("painters")
};

Console.WriteLine("Starting normalization");
using var streamReader = new StreamReader(readPath);
var isFirstLine = true;
while (!streamReader.EndOfStream)
{
    var line = streamReader.ReadLine();
    foreach (var csvProcessor in csvProcessors) 
    {
        var outputLine = isFirstLine ? csvProcessor.ProcessFirstLine() : csvProcessor.ProcessLine(line!);
        File.AppendAllText(csvProcessor.OutputPath, outputLine);
    }
    isFirstLine = false;
}
Console.WriteLine("Finished!");