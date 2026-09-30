using System.Text.Json;
using Towl.Core.Data.Session;
using Towl.Core.Data.Settings;

namespace Towl.Core.Data.Store;

public class JsonDataStore : IDataStore
{
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public VaultSettings LoadSettings()
    {
        try
        {
            return JsonSerializer.Deserialize<VaultSettings>(File.ReadAllText(Constants.ApplicationSettingsFile))!;
        }
        catch (FileNotFoundException)
        {
            var settings = new VaultSettings();
            var jsonString = JsonSerializer.Serialize(settings, _options);
            File.WriteAllText(Constants.ApplicationSettingsFile, jsonString);
        }

        return new VaultSettings();
    }

    public VaultSessionData LoadData()
    {
        try
        {
            return JsonSerializer.Deserialize<VaultSessionData>(File.ReadAllText(Constants.ApplicationDataFile))!;
        }
        catch (FileNotFoundException)
        {
            var data = new VaultSessionData();
            var jsonString = JsonSerializer.Serialize(data, _options);
            File.WriteAllText(Constants.ApplicationDataFile, jsonString);
        }

        return new VaultSessionData();
    }

    public void SaveData(VaultSessionData data)
    {
        lock (data.Lock)
        {
            var jsonString = JsonSerializer.Serialize(data, _options);

            try
            {
                File.WriteAllText(Constants.ApplicationBackupDataFile, jsonString);
                File.Move(Constants.ApplicationBackupDataFile, Constants.ApplicationDataFile, overwrite: true);
            }
            catch
            {
                // Failed to save
            }
        }
    }
}
