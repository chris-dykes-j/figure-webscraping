namespace AlterNormalization.Processors;

public class PaintersCsvProcessor : ArtistsCsvProcessor
{
    public PaintersCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessFirstLine() => "figure_id,painter\n";
    protected override int GetColumnIndex() => 7;

}