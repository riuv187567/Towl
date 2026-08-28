using System.Text.Json;

namespace Towl.Data;

public static class ApplicationDataManager
{
    private readonly static JsonSerializerOptions _options = new() { WriteIndented = true };

    public static ApplicationSettings LoadSettings()
    {
        try
        {
            return JsonSerializer.Deserialize<ApplicationSettings>(File.ReadAllText(Constants.ApplicationSettingsFile))!;
        }
        catch (FileNotFoundException)
        {
            var settings = new ApplicationSettings();
            var jsonString = JsonSerializer.Serialize(settings, _options);
            File.WriteAllText(Constants.ApplicationSettingsFile, jsonString);
        }

        return new ApplicationSettings();
    }

    public static ApplicationSessionData LoadData()
    {
        try
        {
            return JsonSerializer.Deserialize<ApplicationSessionData>(File.ReadAllText(Constants.ApplicationDataFile))!;
        }
        catch (FileNotFoundException)
        {
            var data = new ApplicationSessionData();
            var jsonString = JsonSerializer.Serialize(data, _options);
            File.WriteAllText(Constants.ApplicationDataFile, jsonString);
        }

        return new ApplicationSessionData();
    }

    public static void SaveData(ApplicationSessionData data)
    {
        var jsonString = JsonSerializer.Serialize(data, _options);
        File.WriteAllText(Constants.ApplicationDataFile, jsonString);
    }
}
