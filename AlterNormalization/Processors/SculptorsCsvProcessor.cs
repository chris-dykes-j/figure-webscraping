namespace AlterNormalization.Processors;

public class SculptorsCsvProcessor : ArtistsCsvProcessor
{
    public SculptorsCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessFirstLine() => "name,sculptor\n";
    protected override int GetColumnIndex() => 6;

}