namespace CoordsTool.Core.UserData;

public class UserSettings
{
    public bool ShouldReadFromClipboard { get; set; } = true;
    public bool DisplayTimeColumn { get; set; } = true;
    public bool UseChunkCoordinatesOverworld { get; set; }
    public bool UseChunkCoordinatesNether { get; set; }
    public bool UseChunkCoordinatesEnd { get; set; }
}