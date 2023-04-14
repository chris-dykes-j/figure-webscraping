using Npgsql;

namespace AlterSql.Processor;

public class SculptorCsvProcessor : CsvProcessor
{
    public SculptorCsvProcessor(string tableName, NpgsqlConnection connection) : base(tableName, connection) { }

    protected override void ExecuteSql(string?[] columns)
    {
        var columnData = new Dictionary<string, object?>
        {
            { "figure_id", int.Parse(columns[0]) },
            { "language_code", "ja" },
            { "text", columns[1] }
        };
        InsertData(Connection, "sculptor", columnData);
    }
}