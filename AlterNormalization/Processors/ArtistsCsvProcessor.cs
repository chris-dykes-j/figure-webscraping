using System.Text.RegularExpressions;

namespace AlterNormalization.Processors;

public abstract class ArtistsCsvProcessor : CsvProcessor
{
    protected ArtistsCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessLine(string line)
    {
        var columns = SplitIgnoringQuotes(line, ',');
        var columnIndex = GetColumnIndex();
        var splitArtistsFirst = SplitArtistsByBracket(Regex.Split(columns[columnIndex], @"[】）]\s"));
        var splitArtistsSecond = SplitArtistsByPlus(splitArtistsFirst);

        var result = "";
        foreach (var artist in splitArtistsSecond)
        {
            result += $"{columns[0]},{artist}\n";
        }

        return result;
    }

    private List<string> SplitArtistsByBracket(string[] artists)
    {
        var result = new List<string>();
        foreach (var artist in artists)
        {
            var artistResult = artist;
            if (string.IsNullOrWhiteSpace(artist)) continue;
            if (artist.Contains("原型協力：アルター")) continue;
            if (MissingSquareBracket(artist)) artistResult += '】';
            if (MissingParenthesis(artist)) artistResult += '）';
            result.Add(artistResult);
        }
        return result;
    }

    private List<string> SplitArtistsByPlus(List<string> artists) =>
        artists.SelectMany(artist => artist.Split('＋', StringSplitOptions.RemoveEmptyEntries)).ToList();

    protected bool MissingSquareBracket(string input) => input.Contains('【') && !input.Contains('】');
    protected bool MissingParenthesis(string input) => input.Contains('（') && !input.Contains('）');

    protected abstract int GetColumnIndex();
    
    /* Attempt to deal with cases where credit is given for a particular section to two sculptors.
    var list = sculptor.Split('＋', StringSplitOptions.RemoveEmptyEntries);
    if (MissingSquareBracket(list[0]) || MissingParenthesis(list[0]))
    {
        list[0] += list[1][list[1].IndexOfAny(new[] { '【', '（' })..];
    } 
    */
}