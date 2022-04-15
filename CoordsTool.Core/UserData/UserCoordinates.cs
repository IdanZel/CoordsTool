using CoordsTool.Core.Coordinates;

namespace CoordsTool.Core.UserData;

public record UserCoordinates
{
    public MinecraftCoordinates Coordinates { get; init; }
    public string? Label { get; set; }
    public UserCoordinatesType Type { get; init; }
    public DateTime TimeAdded { get; init; }
}