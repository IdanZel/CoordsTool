﻿namespace CoordsTool.Core.Coordinates;

public readonly record struct MinecraftCoordinates(MinecraftDimension Dimension, double X, double? Y, double Z)
{
    public int BlockX => (int)Math.Floor(X);
    public int BlockZ => (int)Math.Floor(Z);

    public override string ToString() => BlockX + " " +
                                         (Y is not null ? $"{Y} " : string.Empty) +
                                         BlockZ;
}