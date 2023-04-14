using System.Text.RegularExpressions;

namespace AlterNormalization.Processors;

public class PricesCsvProcessor : CsvProcessor
{
    public PricesCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessFirstLine() => "figure_id,price_with_tax,price_without_tax\n";

    public override string ProcessLine(string line)
    {
        var columns = SplitIgnoringQuotes(line, ',');
        // var price = columns[4].Replace("送料手数料別", "").Replace(",", "").Replace("円", "");
        var price = Regex.Replace(columns[4], "送料手数料別|,|円", "");
        const string noTax = "+税";
        const string hasTax = "（税込）";
        const string hasBothPrices = "税抜";
        const string hasTwoPrices = "【通常版】";

        return price switch
        {
            _ when price.Contains(hasTwoPrices) => SplitWithTwoPrices(price.Replace(hasTax, " "), columns[12]),
            _ when price.Contains(noTax) => SplitWithoutTax(price.Replace(noTax, ""), columns[12]),
            _ when price.Contains(hasTax) => SplitWithTax(price.Replace(hasTax, ""), columns[12]),
            _ when price.Contains(hasBothPrices) => SplitWithBothPrices(price, columns[12]),
            _ => ""
        };
    }

    private string SplitWithTwoPrices(string price, string name)
    {
        var lines = price.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        // This sucks and needs addressed. You'll need another column for standard, limited, normal editions.
        return lines.Aggregate("", (current, line) => 
            current + $"{name},{line.Replace("【通常版】", "").Replace("【限定版】", "")},\n");
    }

    private string SplitWithoutTax(string price, string name) => $"{name},,{price}\n";

    private string SplitWithTax(string price, string name) => $"{name},{price},\n";

    private string SplitWithBothPrices(string price, string name) => 
        $"{name},{price.Replace("（税抜", ",").TrimEnd('）')}\n";
    
}