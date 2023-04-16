using System.Text.RegularExpressions;

namespace AlterNormalization.Processors;

public class PricesCsvProcessor : CsvProcessor
{
    public PricesCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessFirstLine() => "figure_id,price_with_tax,price_without_tax,version\n";

    public override string ProcessLine(string line)
    {
        var columns = SplitIgnoringQuotes(line, ',');
        var price = Regex.Replace(columns[4], "送料手数料別|,|円", "");
        const string noTax = "+税";
        const string hasTax = "（税込）";
        const string bothPrices = "税抜";
        const string twoEditions = "【通常版】";

        return price switch
        {
            _ when price.Contains(twoEditions) => SplitWithTwoPrices(price.Replace(hasTax, " "), columns[12]),
            _ when price.Contains(noTax) => SplitWithoutTax(price.Replace(noTax, ""), columns[12]),
            _ when price.Contains(hasTax) => SplitWithTax(price.Replace(hasTax, ""), columns[12]),
            _ when price.Contains(bothPrices) => SplitWithBothPrices(price, columns[12]),
            _ => ""
        };
    }

    private string SplitWithTwoPrices(string price, string name)
    {
        var lines = price.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return lines.Aggregate("", (current, line) =>
        {
            if (line.Contains("【通常版】"))
                return current + $"{name},{line.Replace("【通常版】", "")},,Normal\n";
            if (line.Contains("【限定版】"))
                return current + $"{name},{line.Replace("【限定版】", "")},,Limited\n";
            return line;
        });
    }

    private string SplitWithoutTax(string price, string name) => $"{name},,{price},Standard\n";

    private string SplitWithTax(string price, string name) => $"{name},{price},,Standard\n";

    private string SplitWithBothPrices(string price, string name) => 
        $"{name},{price.Replace("（税抜", ",").TrimEnd('）')},Standard\n";
    
}