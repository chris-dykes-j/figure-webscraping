using Npgsql;

namespace AlterSql.Processor;

public class FigureCsvProcessor : CsvProcessor
{
    public FigureCsvProcessor(string tableName, NpgsqlConnection connection) : base(tableName, connection) { }

    protected override void ExecuteSql(string[] columns)
    {
        InsertFigure(Connection, columns);
        InsertFigureName(Connection, columns);
        InsertSeriesName(Connection, columns);
    }

    void InsertFigure(NpgsqlConnection dbConnection, string[] columns)
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

    void InsertFigureName(NpgsqlConnection dbConnection, string[] columns)
    {
        var columnData = new Dictionary<string, object>
        {
            { "figure_id", int.Parse(columns[0]) },
            { "language_code", "ja" },
            { "text", columns[1] }
        };
        InsertData(dbConnection, "figure_name", columnData);
    }

    void InsertSeriesName(NpgsqlConnection dbConnection, string[] columns)
    {
        var columnData = new Dictionary<string, object>
        {
            { "figure_id", int.Parse(columns[0]) },
            { "language_code", "ja" },
            { "text", columns[2] }
        };
        InsertData(dbConnection, "series_name", columnData);
    }
}