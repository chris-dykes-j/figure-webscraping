using Npgsql;

namespace AlterSql.Processor;

public class FigureCsvProcessor : CsvProcessor
{
    public FigureCsvProcessor(string fileName, NpgsqlConnection connection) : base(fileName, connection) { }

    protected override void ExecuteSql(string?[] columns)
    {
        InsertFigure(Connection, columns);
        InsertFigureName(Connection, columns);
        InsertSeriesName(Connection, columns);
    }

    private void InsertFigure(NpgsqlConnection dbConnection, string?[] columns)
    {
        var scaleValue = GetScaleValue(columns[3]);
        var columnData = new Dictionary<string, object?>
        {
            { "id", int.Parse(columns[0]!) },
            { "scale", columns[3] },
            { "scale_value", scaleValue },
            { "brand", columns[4] },
            { "origin_url", columns[5] }
        };
        InsertData(dbConnection, "figure", columnData);
    }

    private void InsertFigureName(NpgsqlConnection dbConnection, string?[] columns)
    {
        var columnData = new Dictionary<string, object?>
        {
            { "figure_id", int.Parse(columns[0]!) },
            { "language_code", "ja" },
            { "text", columns[1] }
        };
        InsertData(dbConnection, "figure_name", columnData);
    }

    private void InsertSeriesName(NpgsqlConnection dbConnection, string?[] columns)
    {
        var columnData = new Dictionary<string, object?>
        {
            { "figure_id", int.Parse(columns[0]!) },
            { "language_code", "ja" },
            { "text", columns[2] }
        };
        InsertData(dbConnection, "series_name", columnData);
    }

    private static int? GetScaleValue(string? input)
    {
        if (string.IsNullOrEmpty(input)) return null;
        var parts = input.Split('/');
        if (parts.Length != 2) return null;
        return int.TryParse(parts[1], out var x) ? x : null;
    }
}