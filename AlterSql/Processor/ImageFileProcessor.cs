using Npgsql;

namespace AlterSql.Processor;

public class ImageFileProcessor : CsvProcessor
{
    
    public ImageFileProcessor(string fileName, NpgsqlConnection connection) : base(fileName, connection) { }

    protected override void ExecuteSql(string?[] columns)
    {
        var imageLinks = GetImageFileNames(columns[1]!);
        foreach (var link in imageLinks)
        {
            var columnData = new Dictionary<string, object?>
            {
                { "figure_id", int.Parse(columns[0]!) },
                { "image_url", link }
            };
            InsertData(Connection, "image_url", columnData);
        }
    }

    private List<string> GetImageFileNames(string figureName)
    {
        if (figureName.Equals("Fate/EXTRA セイバーエクストラ　水着Ver."))
        {
            figureName = figureName.Replace("Fate/", "2017/Fate/"); 
        }
        
        const string path = "/run/media/chris/Extra/Figures/Alter/";
        var directory = Directory
            .GetDirectories(path, figureName, SearchOption.AllDirectories)
            .FirstOrDefault() 
            ?? figureName switch
            {
                "シリアス　青雲映す碧波Ver." => "/run/media/chris/Extra/Figures/Alter/2022/シリアス　青雲映す碧波Ver. ",
                "食蜂 操祈　スク水ニーソVer." => "/run/media/chris/Extra/Figures/Alter/2022/食蜂 操祈　スク水ニーソVer. ",
                "モモ・ベリア・デビルーク　パジャマVer." => "/run/media/chris/Extra/Figures/Alter/2021/モモ・ベリア・デビルーク　パジャマVer. ",
                "Fate/EXTRA セイバーエクストラ　水着Ver." => "/run/media/chris/Extra/Figures/Alter/2017/Fate/EXTRA セイバーエクストラ　水着Ver.",
                _ => null
            };
        
        return Directory.GetFiles(directory!).Select(image => image.Replace(path, "anime-figures.moe/")).ToList();
    }
}