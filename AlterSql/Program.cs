﻿using AlterSql.Processor;
using Npgsql;

const string connectionString = "Host=localhost;Username=chris;Password=;Database=figures";
const string initScriptPath = "/home/chris/RiderProjects/FigureWebScraper/AlterSql/init.sql";

// Init tables
using var connection = new NpgsqlConnection(connectionString);
try
{
    connection.Open();
    ExecuteInitSqlScript(connection, initScriptPath);
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e}");
}

var csvProcessors = new List<CsvProcessor>
{
    new FigureCsvProcessor("figures", connection),
    new CharacterCsvProcessor("characters", connection),
    //new MaterialCsvProcessor("materials", connection),
    //new MeasurementCsvProcessor("measurements", connection),
    //new BlogUrlCsvProcessor("blog-urls", connection),
    //new PainterCsvProcessor("painters", connection),
    //new SculptorCsvProcessor("sculptors", connection),
    //new ReleaseDateCsvProcessor("release-dates", connection),
    new PriceCsvProcessor("prices", connection)
};

foreach (var csvProcessor in csvProcessors)
{
    csvProcessor.ReadAndProcess();
}

connection.Close();

void ExecuteInitSqlScript(NpgsqlConnection dbConnection, string scriptPath)
{
    var scriptContent = File.ReadAllText(scriptPath);
    var commands = scriptContent.Split(';', StringSplitOptions.RemoveEmptyEntries);

    foreach (var command in commands)
    {
        using var sqlCommand = new NpgsqlCommand(command, dbConnection);
        sqlCommand.ExecuteNonQuery();
    }
}