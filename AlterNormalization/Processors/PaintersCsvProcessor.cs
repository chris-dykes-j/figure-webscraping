namespace AlterNormalization.Processors;

public class PaintersCsvProcessor : ArtistsCsvProcessor
{
    public PaintersCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessFirstLine() => "name,painter\n";
    protected override int GetColumnIndex() => 7;

}