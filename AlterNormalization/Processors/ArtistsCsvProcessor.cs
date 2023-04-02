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

        return splitArtistsSecond.Aggregate("", (current, artist) => current + $"{columns[0]},{artist}\n");
    }

    private IEnumerable<string> SplitArtistsByBracket(IEnumerable<string> artists)
    {
        var result = new List<string>();
        foreach (var artist in artists)
        {
            var artistResult = artist;
            if (string.IsNullOrWhiteSpace(artist)) continue;
            if (artist.Contains("原型協力：アルター")) continue;
            if (string.Equals(artist, "—") || string.Equals(artist, "―")) continue;
            if (MissingSquareBracket(artist)) artistResult += '】';
            if (MissingParenthesis(artist)) artistResult += '）';
            result.Add(artistResult);
        }
        return result;
    }

    private IEnumerable<string> SplitArtistsByPlus(IEnumerable<string> artists) =>
        artists.SelectMany(artist =>
            {
                var list = artist.Split('＋', StringSplitOptions.RemoveEmptyEntries);
                if (list.Length <= 1) return list;
                if (!list[0].Contains('【') && list[1].Contains('【'))
                {
                    list[0] += list[1][list[1].IndexOf('【')..];
                }
                return list;
            })
            .ToList();

    private bool MissingSquareBracket(string input) => input.Contains('【') && !input.Contains('】');
    private bool MissingParenthesis(string input) => input.Contains('（') && !input.Contains('）');

    protected abstract int GetColumnIndex();
}