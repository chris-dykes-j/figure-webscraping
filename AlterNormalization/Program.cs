
using System.Globalization;
using System.Text.RegularExpressions;
using AlterNormalization;
using CsvHelper;

const string readPath = "/home/chris/RiderProjects/FigureWebScraper/AlterScraper/alter-jp.csv";

string CsvPath(string tableName) => 
    $"/home/chris/RiderProjects/FigureWebScraper/AlterNormalization/alter-{tableName}-jp.csv";

using (var streamWriter = File.CreateText(CsvPath("figures")))
{
    streamWriter.WriteLine(CsvPath("figures"), "name,series,character,scale,size,brand,origin_url");
}

using var streamReader = new StreamReader(readPath);

Console.WriteLine("Starting normalization");
while (!streamReader.EndOfStream)
{
    var line = streamReader.ReadLine();
    var columns = line!.Split(',');
    var scaleRegex = new Regex(@"^1\//1-9]");
    var scale = scaleRegex.Match(columns[5]).Value;
    var size = scaleRegex.Replace(columns[5], "");
    var figuresValues = $"{columns[0]},{columns[1]},{columns[2]},{scale},{size},{columns[9]},{columns[10]}";
    File.WriteAllText(CsvPath("figures"), figuresValues);
    Console.WriteLine(figuresValues);
}

// Handle repeating groups in release, price, sculptor, painter, material, blog_url

// Separate year and month.

// Separate scale from size.
