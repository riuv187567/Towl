using System.Text.Json;
using Towl.Core.Data.Session;
using Towl.Core.Data.Settings;
using Towl.Core.Utils;

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

    public VaultData LoadData()
    {
        try
        {
            return JsonSerializer.Deserialize<VaultData>(File.ReadAllText(Constants.ApplicationDataFile))!;
        }
        catch (FileNotFoundException)
        {
            var data = new VaultData();
            var jsonString = JsonSerializer.Serialize(data, _options);
            File.WriteAllText(Constants.ApplicationDataFile, jsonString);
        }

        return new VaultData();
    }

    public void SaveData(VaultData data)
    {
        lock (data.Lock)
        {
            var jsonString = JsonSerializer.Serialize(data, _options);

            FileUtils.WriteFileSafe(Constants.ApplicationDataFile, jsonString, Constants.ApplicationBackupDataFile);
        }
    }
}
