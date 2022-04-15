namespace CoordsTool.Core.Coordinates;

public readonly record struct MinecraftCoordinates(MinecraftDimension Dimension, double X, double Z)
{
    public override string ToString() => $"{X} {Z}";
}