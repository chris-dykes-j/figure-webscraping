namespace AlterNormalization.Processors;

public class CharactersCsvProcessor : CsvProcessor
{
    public CharactersCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessFirstLine() => "name,character";

    public override string ProcessLine(string line)
    {
        var columns = SplitIgnoringQuotes(line, ',');
        var charactersColumn = SplitCharacters(columns);
        
        return charactersColumn.Aggregate("", (current, character) =>
        {
            if (IsRyzaCharacter(character))
                return current.TrimEnd('\n').Trim() + $"{character}\n";
            return current + $"{columns[0]},{character}\n";
        });
    }

    private IEnumerable<string> SplitCharacters(List<string> columns)
    {
        return string.IsNullOrWhiteSpace(columns[2])
            ? columns[0].Split('&', StringSplitOptions.RemoveEmptyEntries)
            : columns[2].Split('&', StringSplitOptions.RemoveEmptyEntries);
    }

    private bool IsRyzaCharacter(string character) => character.Contains("the Secret Hideout");
}