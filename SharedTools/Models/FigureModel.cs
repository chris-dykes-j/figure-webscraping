namespace SharedTools.Models;

public class FigureModel
{
   public string Name { get; set; } = null!;

   public string Character { get; set; } = null!;

   public string Brand { get; set; } = null!;

   public int ReleasePrice { get; set; } 
   
   public List<string> ReleaseDate { get; set; } = null!; // Will be multiple
   
   public string? Scale { get; set; }
   
   public string? Size { get; set; }
   
   public string? Series { get; set; }
   
   public string? ProductLine { get; set; }
   
   public string? Sculptor { get; set; } // May have multiple
   
   public string? Painter { get; set; } // May have multiple
   
   public string? Material { get; set; } // Will likely need to normalize, but PVC&ABS is usually not separated.
   
   public long? JanCode { get; set; }
}