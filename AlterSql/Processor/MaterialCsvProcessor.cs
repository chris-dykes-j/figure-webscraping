using Npgsql;

namespace AlterSql.Processor;

public class MaterialCsvProcessor : CsvProcessor
{
    public MaterialCsvProcessor(string fileName, NpgsqlConnection connection) : base(fileName, connection) { }

    protected override void ExecuteSql(string?[] columns)
    {
        var columnData = new Dictionary<string, object?>
        {
            { "figure_id", int.Parse(columns[0]!) },
            { "material", columns[1] }
        };
        InsertData(Connection, "material", columnData);
    }
}