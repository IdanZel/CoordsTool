namespace CoordsTool.Core.Coordinates;

public static class CoordinatesExtensions
{
    public static MinecraftCoordinates ConvertDimension(this MinecraftCoordinates coordinates) =>
        coordinates.Dimension switch
        {
            MinecraftDimension.Overworld =>
                new MinecraftCoordinates(MinecraftDimension.Nether, coordinates.X / 8, coordinates.Z / 8),

            MinecraftDimension.Nether =>
                new MinecraftCoordinates(MinecraftDimension.Overworld, coordinates.X * 8, coordinates.Z * 8),

            MinecraftDimension.End or _ => coordinates
        };

    public static MinecraftCoordinates ToChunkCoordinates(this MinecraftCoordinates coordinates) =>
        new(coordinates.Dimension, coordinates.X / 16, coordinates.Z / 16);
}