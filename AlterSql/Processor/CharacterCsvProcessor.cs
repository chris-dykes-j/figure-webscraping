using Npgsql;

namespace AlterSql.Processor;

public class CharacterCsvProcessor : CsvProcessor
{
    public CharacterCsvProcessor(string fileName, NpgsqlConnection connection) : base(fileName, connection) { }

    protected override void ExecuteSql(string?[] columns)
    {
        var columnData = new Dictionary<string, object?>
        {
            { "figure_id", int.Parse(columns[0]!) },
            { "language_code", "ja" },
            { "text", columns[1] }
        };
        InsertData(Connection, "character_name", columnData);
    }
}