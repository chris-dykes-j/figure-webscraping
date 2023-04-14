using Npgsql;

namespace AlterSql.Processor;

public class BlogUrlCsvProcessor : CsvProcessor
{
    public BlogUrlCsvProcessor(string fileName, NpgsqlConnection connection) : base(fileName, connection) { }

    protected override void ExecuteSql(string?[] columns)
    {
        var columnData = new Dictionary<string, object?>
        {
            { "figure_id", int.Parse(columns[0]!) },
            { "blog_url", columns[1] }
        };
        InsertData(Connection, "blog_url", columnData);
    }
}