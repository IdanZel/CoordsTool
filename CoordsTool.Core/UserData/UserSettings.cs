namespace CoordsTool.Core.UserData;

public class UserSettings
{
    public bool ShouldReadFromClipboard { get; set; } = true;
    public bool DisplayTimeColumn { get; set; } = true;
    public bool AlwaysOnTop { get; set; }
    public bool UseChunkCoordinatesOverworld { get; set; }
    public bool UseChunkCoordinatesNether { get; set; }
    public bool UseChunkCoordinatesEnd { get; set; }
    public bool DisplayYLevel { get; set; }

    public double WindowHeight { get; set; }
    public double WindowWidth { get; set; }
    public double TopPosition { get; set; }
    public double LeftPosition { get; set; }
    public double RelativeCoordinatesColumnWidth { get; set; }
    public double RelativeLabelsColumnWidth { get; set; }
    public double RelativeTimeColumnWidth { get; set; }
}