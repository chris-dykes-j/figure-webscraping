using System.Text.RegularExpressions;

namespace AlterNormalization.Processors;

public abstract class ArtistsCsvProcessor : CsvProcessor
{
    protected ArtistsCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessLine(string line)
    {
        var columns = SplitIgnoringQuotes(line, ',');
        var splitArtistsFirst = SplitArtistsByBracket(Regex.Split(columns[GetColumnIndex()], @"[】）]\s"));
        var splitArtistsSecond = SplitArtistsByPlus(splitArtistsFirst);

        return splitArtistsSecond.Aggregate("", (current, artist) => current + $"{columns[12]},{artist}\n");
    }

    private IEnumerable<string> SplitArtistsByBracket(IEnumerable<string> artists)
    {
        var result = new List<string>();
        foreach (var artist in artists)
        {
            if (string.IsNullOrWhiteSpace(artist)) continue;
            if (string.Equals(artist, "—") || string.Equals(artist, "―")) continue;
            
            var artistResult = artist;
            if (MissingSquareBracket(artistResult)) artistResult += '】';
            if (MissingParenthesis(artistResult)) artistResult += '）';
            if (artist.Contains("原型協力：アルター")) artistResult = artistResult.Replace("原型協力：アルター", "");
            if (artist.Contains("彩色協力：アルター")) artistResult = artistResult.Replace("彩色協力：アルター", "");
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
                    var index = list[1].IndexOf('【');
                    list[0] += list[1][index..];
                }
                return list;
            }).ToList();

    private bool MissingSquareBracket(string input) => input.Contains('【') && !input.Contains('】');
    private bool MissingParenthesis(string input) => input.Contains('（') && !input.Contains('）');

    protected abstract int GetColumnIndex();
}