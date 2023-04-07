namespace AlterNormalization.Processors;

public class CharactersCsvProcessor : CsvProcessor
{
    public CharactersCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessFirstLine() => "name,character\n";

    private int i = 0;
    public override string ProcessLine(string line)
    {
        var columns = SplitIgnoringQuotes(line, ',');
        var charactersColumn = GetCharacters(columns);
        
        return charactersColumn.Aggregate("", (current, character) => //IsTwoCharacters(character) 
            $"{current}{columns[0]},{character.TrimStart()}\n");
    }

    private IEnumerable<string> GetCharacters(List<string> columns)
    {
        if (IsRyzaCharacter(columns[2])) return GetRyzaCharacter(columns[2]);
        if (IsEdgeCaseCharacter(columns[0], columns[2])) return GetEdgeCaseCharacters(columns[0], columns[2]);
        return string.IsNullOrWhiteSpace(columns[2])
            ? columns[0].Split(new[]{'&','＆'}, StringSplitOptions.RemoveEmptyEntries)
            : columns[2].Split(new[]{'&', '＆'}, StringSplitOptions.RemoveEmptyEntries);
    }

    private bool IsRyzaCharacter(string character) => character.Contains("Atelier Ryza: Ever Darkness & the Secret Hideout");
    private bool IsEdgeCaseCharacter(string nameColumn, string characterColumn)
    {
        var character = string.IsNullOrWhiteSpace(characterColumn) ? nameColumn : characterColumn;
        return character.Contains("AZUR LANE IJN Atago & IJN Takao") ||
               character.Contains("ALVIS & LAVIE BY LASTEXILE") ||
               character.Contains("シャマル & ザフィーラ 仔犬Ver.") ||
               character.Contains("ユーリ・ローウェル　心の中の聖騎士様Ver.＆ラピード");
    }

    private IEnumerable<string> GetRyzaCharacter(string characters) => new[] { characters };

    private IEnumerable<string> GetEdgeCaseCharacters(string nameColumn, string characterColumn)
    {
        var characters = string.IsNullOrWhiteSpace(characterColumn) ? nameColumn : characterColumn;
        return characters switch
        {
            _ when characters.Contains("AZUR LANE IJN Atago & IJN Takao") => new [] { "AZUR LANE IJN Atago", "AZUR LANE IJN Takao" },
            _ when characters.Contains("ALVIS & LAVIE BY LASTEXILE") => new [] { "ALVIS BY LASTEXILE", "LAVIE BY LASTEXILE" },
            _ when characters.Contains("シャマル & ザフィーラ 仔犬Ver.") => new [] {"シャマル", "ザフィーラ" },
            _ when characters.Contains("ユーリ・ローウェル　心の中の聖騎士様Ver.＆ラピード") => new []{ "ユーリ・ローウェル", "ラピード"},
            _ => new [] {""}
        };
    }
}