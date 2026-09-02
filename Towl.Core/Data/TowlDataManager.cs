using System.Text.Json;
using Towl.Core.Data.Session;
using Towl.Core.Data.Settings;

namespace Towl.Core.Data;

public static class TowlDataManager
{
    private readonly static JsonSerializerOptions _options = new() { WriteIndented = true };

    public static TowlSettings LoadSettings()
    {
        try
        {
            return JsonSerializer.Deserialize<TowlSettings>(File.ReadAllText(Constants.ApplicationSettingsFile))!;
        }
        catch (FileNotFoundException)
        {
            var settings = new TowlSettings();
            var jsonString = JsonSerializer.Serialize(settings, _options);
            File.WriteAllText(Constants.ApplicationSettingsFile, jsonString);
        }

        return new TowlSettings();
    }

    public static TowlSessionData LoadData()
    {
        try
        {
            return JsonSerializer.Deserialize<TowlSessionData>(File.ReadAllText(Constants.ApplicationDataFile))!;
        }
        catch (FileNotFoundException)
        {
            var data = new TowlSessionData();
            var jsonString = JsonSerializer.Serialize(data, _options);
            File.WriteAllText(Constants.ApplicationDataFile, jsonString);
        }

        return new TowlSessionData();
    }

    public static void SaveData(TowlSessionData data)
    {
        var jsonString = JsonSerializer.Serialize(data, _options);
        File.WriteAllText(Constants.ApplicationDataFile, jsonString);
    }
}
