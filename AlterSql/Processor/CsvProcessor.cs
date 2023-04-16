using System.Data;
using Npgsql;

namespace AlterSql.Processor;

public abstract class CsvProcessor
{
    private readonly string _readPath;
    protected readonly NpgsqlConnection Connection;

    protected CsvProcessor(string fileName, NpgsqlConnection connection)
    {
        _readPath = $"/home/chris/RiderProjects/FigureWebScraper/AlterNormalization/Output/alter-{fileName}-jp.csv";
        Connection = connection;
    }

    public void ReadAndProcess()
    {
        using var streamReader = new StreamReader(_readPath);
        streamReader.ReadLine(); // Skip first line.
        while (!streamReader.EndOfStream)
        {
            var line = streamReader.ReadLine();
            if (line == null) break;

            string?[] columns = line.Split(',');
            using var transaction = Connection.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                ExecuteSql(columns);
                transaction.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error processing line: {line}\n{e}");
                transaction.Rollback();
            }
        }
    }

    protected void InsertData(NpgsqlConnection dbConnection, string tableName, Dictionary<string, object?> columnData)
    {
        var columnNames = string.Join(", ", columnData.Keys);
        var parameterNames = string.Join(", ", columnData.Keys.Select(k => "@" + k));
        var insertQuery = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameterNames});";
        using var command = new NpgsqlCommand(insertQuery, dbConnection);
        foreach (var column in columnData)
        {
            command.Parameters.AddWithValue(column.Key, column.Value ?? DBNull.Value);
        }
        command.ExecuteNonQuery();
    }

    protected abstract void ExecuteSql(string?[] columns);
}