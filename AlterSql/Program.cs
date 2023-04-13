using System.Data;
using Npgsql;

const string readPath = "/home/chris/RiderProjects/FigureWebScraper/AlterNormalization/Output/alter-figures-jp.csv";
const string connectionString = "Host=localhost;Username=chris;Password=;Database=figures";
const string initScriptPath = "/home/chris/RiderProjects/FigureWebScraper/AlterSql/init.sql";

using var streamReader = new StreamReader(readPath);
streamReader.ReadLine(); // Skip first line.

using var connection = new NpgsqlConnection(connectionString);
try
{
    connection.Open();
    ExecuteSqlScript(connection, initScriptPath);
}
catch (Exception e)
{
    Console.WriteLine($"Error: {e}");
}

while (!streamReader.EndOfStream)
{
    var line = streamReader.ReadLine();
    if (line == null) break;

    var columns = SplitIgnoringQuotes(line, ',');
    using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
    try
    {
        InsertFigure(connection, columns);
        InsertFigureName(connection, columns);
        InsertSeriesName(connection, columns);
        transaction.Commit();
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error processing line: {line}\n{e}");
        transaction.Rollback();
    }
}

connection.Close();

void ExecuteSqlScript(NpgsqlConnection dbConnection, string scriptPath)
{
    var scriptContent = File.ReadAllText(scriptPath);
    var commands = scriptContent.Split(';', StringSplitOptions.RemoveEmptyEntries);

    foreach (var command in commands)
    {
        using var sqlCommand = new NpgsqlCommand(command, dbConnection);
        sqlCommand.ExecuteNonQuery();
    }
}

void InsertData(NpgsqlConnection dbConnection, string tableName, Dictionary<string, object> columnData)
{
    var columnNames = string.Join(", ", columnData.Keys);
    var parameterNames = string.Join(", ", columnData.Keys.Select(k => "@" + k));
    var insertQuery = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameterNames});";
    using var command = new NpgsqlCommand(insertQuery, dbConnection);
    foreach (var column in columnData)
    {
        command.Parameters.AddWithValue(column.Key, column.Value);
    }
    command.ExecuteNonQuery();
}

void InsertFigure(NpgsqlConnection dbConnection, List<string> columns)
{
    var columnData = new Dictionary<string, object>
    {
        { "id", int.Parse(columns[0]) },
        { "scale", columns[3] },
        { "brand", columns[4] },
        { "origin_url", columns[5] }
    };
    InsertData(dbConnection, "figure", columnData);
}

void InsertFigureName(NpgsqlConnection dbConnection, List<string> columns)
{
    var columnData = new Dictionary<string, object>
    {
        { "figure_id", int.Parse(columns[0]) },
        { "language_code", "ja" },
        { "text", columns[1] }
    };
    InsertData(dbConnection, "figure_name", columnData);
}

void InsertSeriesName(NpgsqlConnection dbConnection, List<string> columns)
{
    var columnData = new Dictionary<string, object>
    {
        { "figure_id", int.Parse(columns[0]) },
        { "language_code", "ja" },
        { "text", columns[2] }
    };
    InsertData(dbConnection, "series_name", columnData);
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