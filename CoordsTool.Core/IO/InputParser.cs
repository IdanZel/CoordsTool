using CoordsTool.Core.Coordinates;

namespace CoordsTool.Core.IO;

public static class InputParser
{
    private static readonly Dictionary<string, MinecraftDimension> DimensionsMap = new()
    {
        ["overworld"] = MinecraftDimension.Overworld,
        ["the_nether"] = MinecraftDimension.Nether,
        ["the_end"] = MinecraftDimension.End
    };

    public static bool TryParseManualInput(string input, MinecraftDimension dimension,
        out MinecraftCoordinates coordinates)
    {
        var inputParts = input.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

        double x = default;
        double y = double.NaN;
        double z = default;

        var tryParse = inputParts.Length switch
        {
            // Try parse X Z
            2 => double.TryParse(inputParts[0], out x) &&
                 double.TryParse(inputParts[1], out z),

            // Try parse X Y Z
            3 => double.TryParse(inputParts[0], out x) &&
                 double.TryParse(inputParts[1], out y) &&
                 double.TryParse(inputParts[2], out z),

            _ => false
        };

        if (!tryParse)
        {
            coordinates = default;
            return false;
        }

        coordinates = new MinecraftCoordinates(dimension, x, !double.IsNaN(y) ? y : null, z);
        return true;
    }

    public static bool TryParseF3C(string input, out MinecraftCoordinates coordinates)
    {
        var inputParts = input.Split();

        if (inputParts.Length != 11 ||
            !TryParseMinecraftDimension(inputParts[2], out var dimension) ||
            !double.TryParse(inputParts[6], out var x) ||
            !double.TryParse(inputParts[7], out var y) ||
            !double.TryParse(inputParts[8], out var z))
        {
            coordinates = default;
            return false;
        }

        coordinates = new MinecraftCoordinates(dimension, x, !double.IsNaN(y) ? y : null, z);
        return true;
    }

    private static bool TryParseMinecraftDimension(string input, out MinecraftDimension dimension)
    {
        var dimensionString = input.Replace("minecraft:", string.Empty);
        return DimensionsMap.TryGetValue(dimensionString, out dimension);
    }
}