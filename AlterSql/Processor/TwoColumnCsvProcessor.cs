using Npgsql;

namespace AlterSql.Processor;

public class TwoColumnCsvProcessor : CsvProcessor
{
    private readonly string _tableName;

    public TwoColumnCsvProcessor(string fileName, string tableName, NpgsqlConnection connection) : base(fileName, connection)
    {
        _tableName = tableName;
    }

    protected override void ExecuteSql(string?[] columns)
    {
        var columnData = new Dictionary<string, object?>
        {
            { "figure_id", int.Parse(columns[0]!) },
            { "language_code", "ja" },
            { "text", columns[1] }
        };
        InsertData(Connection, _tableName, columnData);
    }
}