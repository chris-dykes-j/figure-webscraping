namespace AlterNormalization.Processors;

public class PriceCsvProcessor : CsvProcessor
{
    public PriceCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessFirstLine() => "name,price_with_tax,price_without_tax\n";

    public override string ProcessLine(string line)
    {
        var result = "";
        var columns = SplitIgnoringQuotes(line, ',');
        var price = columns[4].Replace("送料手数料別", "").Replace(",", "");
        // 送料手数料別
        const string noTax = "+税";
        const string hasTax = "（税込）";
        const string hasBothPrices = "税抜";
        const string hasTwoPrices = "【通常版】";

        if (price.Contains(hasTwoPrices))
        {
            return SplitWithTwoPrices(price.Replace(hasTax, " "), columns[0]);
        }

        if (price.Contains(noTax))
        {
            result += SplitWithoutTax(price.Replace(noTax, ""));
        }
        else if (price.Contains(hasTax))
        {
            result += SplitWithTax(price.Replace(hasTax, ""));
        }
        else if (price.Contains(hasBothPrices))
        {
            result += SplitWithBothPrices(price);
        }

        return $"{columns[0]},{result}\n";
    }

    private string SplitWithTwoPrices(string price, string name)
    {
        var result = "";
        var lines = price.Split(' ');
        foreach (var line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                result += $"{name},{line},\n";
            }
        }
        return result;
    }

    private string SplitWithoutTax(string price) => $",{price}";

    private string SplitWithTax(string price) => $"{price},";

    private string SplitWithBothPrices(string price) => price.Replace("（税抜", ",").TrimEnd('）');
}