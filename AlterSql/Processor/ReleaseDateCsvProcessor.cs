using Npgsql;

namespace AlterSql.Processor;

public class ReleaseDateCsvProcessor : CsvProcessor
{
    public ReleaseDateCsvProcessor(string tableName, NpgsqlConnection connection) : base(tableName, connection) { }

    protected override void ExecuteSql(string?[] columns)
    {
        var columnData = new Dictionary<string, object?>
        {
            { "figure_id", int.Parse(columns[0]) },
            { "language_code", "ja" },
            { "release_year", columns[1] },
            { "release_month", columns[2] }
        };
        InsertData(Connection, "release_date", columnData);
    }
}