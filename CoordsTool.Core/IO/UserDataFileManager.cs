using System.Text.Json;
using CoordsTool.Core.UserData;

namespace CoordsTool.Core.IO;

public static class UserDataFileManager
{
    private static readonly string UserDataFolderPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CoordsTool");

    private static readonly string UserDataFilePath = Path.Combine(UserDataFolderPath, "UserData.json");
    private static readonly string UserSettingsFilePath = Path.Combine(UserDataFolderPath, "UserSettings.json");

    public static List<UserCoordinates> ReadCoordinatesList()
    {
        if (!File.Exists(UserDataFilePath))
        {
            return new List<UserCoordinates>();
        }

        var data = File.ReadAllText(UserDataFilePath);
        return JsonSerializer.Deserialize<List<UserCoordinates>>(data) ?? new List<UserCoordinates>();
    }

    public static void WriteCoordinatesList(IEnumerable<UserCoordinates> coordinatesList)
    {
        CreateUserDataFolderIfNotExists();

        var data = JsonSerializer.Serialize(coordinatesList as List<UserCoordinates> ??
                                            new List<UserCoordinates>(coordinatesList));
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
        CreateUserDataFolderIfNotExists();

        var data = JsonSerializer.Serialize(settings);
        File.WriteAllText(UserSettingsFilePath, data);
    }

    private static void CreateUserDataFolderIfNotExists()
    {
        if (!Directory.Exists(UserDataFolderPath))
        {
            Directory.CreateDirectory(UserDataFolderPath);
        }
    }
}