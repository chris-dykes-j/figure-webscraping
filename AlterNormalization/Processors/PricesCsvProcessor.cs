namespace AlterNormalization.Processors;

public class PricesCsvProcessor : CsvProcessor
{
    public PricesCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessFirstLine() => "name,price_with_tax,price_without_tax\n";

    public override string ProcessLine(string line)
    {
        var columns = SplitIgnoringQuotes(line, ',');
        var price = columns[4].Replace("送料手数料別", "").Replace(",", "");
        const string noTax = "+税";
        const string hasTax = "（税込）";
        const string hasBothPrices = "税抜";
        const string hasTwoPrices = "【通常版】";

        return price switch
        {
            _ when price.Contains(hasTwoPrices) => SplitWithTwoPrices(price.Replace(hasTax, " "), columns[0]),
            _ when price.Contains(noTax) => SplitWithoutTax(price.Replace(noTax, ""), columns[0]),
            _ when price.Contains(hasTax) => SplitWithTax(price.Replace(hasTax, ""), columns[0]),
            _ when price.Contains(hasBothPrices) => SplitWithBothPrices(price, columns[0]),
            _ => ""
        };
    }

    private string SplitWithTwoPrices(string price, string name)
    {
        var lines = price.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return lines.Aggregate("", (current, line) => current + $"{name},{line},\n");
    }

    private string SplitWithoutTax(string price, string name) => $"{name},,{price}\n";

    private string SplitWithTax(string price, string name) => $"{name},{price},\n";

    private string SplitWithBothPrices(string price, string name) => 
        $"{name},{price.Replace("（税抜", ",").TrimEnd('）')}\n";
}