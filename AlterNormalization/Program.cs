using AlterNormalization.Processors;

const string readPath = "/home/chris/RiderProjects/FigureWebScraper/AlterScraper/alter-jp.csv";

var csvProcessors = new List<CsvProcessor> 
{/*
    new FiguresCsvProcessor("figures"),
    new ReleaseDateCsvProcessor("release-dates"),
    new PriceCsvProcessor("prices"),
    new BlogUrlCsvProcessor("blog-urls"),
    new MeasurementCsvProcessor("measurements"),
    new SculptorCsvProcessor("sculptors"),*/
    new MaterialsCsvProcessor("materials")
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