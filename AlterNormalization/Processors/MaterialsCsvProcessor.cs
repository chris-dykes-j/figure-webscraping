namespace AlterNormalization.Processors;

public class MaterialsCsvProcessor : CsvProcessor
{
    public MaterialsCsvProcessor(string outPath) : base(outPath) { }

    public override string ProcessFirstLine() => "name,material\n";

    public override string ProcessLine(string line)
    {
        var columns = SplitIgnoringQuotes(line, ',');
        var materials = GetMaterials(columns[8]);
        var result = "";
        foreach (var material in materials)
        {
            result += $"{columns[0]},{material}\n";
        }

        return result;
    }
    
    private List<string> GetMaterials(string materials)
    {
        var result = new List<string> { "PVC" }; // We can assume that the figures contain PVC.
        if (materials.Contains("PVC&ABS")) result.Add("ABS");

        return result;
    }
}