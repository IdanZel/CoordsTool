using System.Text.Json;
using CoordsTool.Core.UserData;

namespace CoordsTool.Core.IO;

public static class UserDataFileManager
{
    private const string UserDataFilePath = "UserData.json";
    private const string UserSettingsFilePath = "UserSettings.json";

    public static List<UserCoordinates> ReadCoordinatesList()
    {
        if (!File.Exists(UserDataFilePath))
        {
            return new List<UserCoordinates>();
        }

        var data = File.ReadAllText(UserDataFilePath);
        return JsonSerializer.Deserialize<List<UserCoordinates>>(data) ?? new List<UserCoordinates>();
    }

    public static void WriteCoordinatesList(List<UserCoordinates> coordinatesList)
    {
        var data = JsonSerializer.Serialize(coordinatesList);
        File.WriteAllText(UserDataFilePath, data);
    }

    public static UserSettings ReadSettings()
    {
        if (!File.Exists(UserSettingsFilePath))
        {
            return new UserSettings();
        }

        var data = File.ReadAllText(UserSettingsFilePath);
        return JsonSerializer.Deserialize<UserSettings>(data) ?? new UserSettings();
    }

    public static void WriteSettings(UserSettings settings)
    {
        var data = JsonSerializer.Serialize(settings);
        File.WriteAllText(UserSettingsFilePath, data);
    }
}