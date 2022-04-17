namespace CoordsTool.Core.Coordinates;

public static class CoordinatesExtensions
{
    public static MinecraftCoordinates ConvertDimension(this MinecraftCoordinates coordinates) =>
        coordinates.Dimension switch
        {
            MinecraftDimension.Overworld =>
                new MinecraftCoordinates(MinecraftDimension.Nether,
                    Math.Floor(coordinates.BlockX / 8.0),
                    Math.Floor(coordinates.BlockZ / 8.0)),

            MinecraftDimension.Nether =>
                new MinecraftCoordinates(MinecraftDimension.Overworld,
                    coordinates.BlockX * 8,
                    coordinates.BlockZ * 8),

            MinecraftDimension.End or _ => coordinates
        };

    public static MinecraftCoordinates ToChunkCoordinates(this MinecraftCoordinates coordinates) =>
        new(coordinates.Dimension,
            Math.Floor(coordinates.BlockX / 16.0),
            Math.Floor(coordinates.BlockZ / 16.0));
}