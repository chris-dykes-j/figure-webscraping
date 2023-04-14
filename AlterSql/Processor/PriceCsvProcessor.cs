using Npgsql;

namespace AlterSql.Processor;

public class PriceCsvProcessor : CsvProcessor
{
    public PriceCsvProcessor(string fileName, NpgsqlConnection connection) : base(fileName, connection) { }

    protected override void ExecuteSql(string?[] columns)
    {
        int? priceWithTax = int.TryParse(columns[1], out var withTax) ? withTax : null;
        int? priceWithoutTax = int.TryParse(columns[2], out var withoutTax) ? withoutTax : null;
        var columnData = new Dictionary<string, object?>
        {
            { "figure_id", int.Parse(columns[0]!) },
            { "price_with_tax", priceWithTax },
            { "price_without_tax", priceWithoutTax }
        };
        InsertData(Connection, "price", columnData);
    }
}