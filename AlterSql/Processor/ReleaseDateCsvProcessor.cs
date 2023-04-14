using Npgsql;

namespace AlterSql.Processor;

public class ReleaseDateCsvProcessor : CsvProcessor
{
    public ReleaseDateCsvProcessor(string fileName, NpgsqlConnection connection) : base(fileName, connection) { }

    protected override void ExecuteSql(string?[] columns)
    {
        var columnData = new Dictionary<string, object?>
        {
            { "figure_id", int.Parse(columns[0]!) },
            { "release_year", int.Parse(columns[1]!) },
            { "release_month", int.Parse(columns[2]!) }
        };
        InsertData(Connection, "release_date", columnData);
    }
}