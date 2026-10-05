using System.Text.Json;
using Towl.Core.Data.Session;
using Towl.Core.Data.Settings;
using Towl.Core.Utils;

namespace Towl.Core.Data.Store;

public class JsonDataStore : IDataStore
{
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public void TryLoadData(out VaultData vaultData)
    {
        var dataExists = File.Exists(Constants.ApplicationDataFile);
        var backupExists = File.Exists(Constants.ApplicationBackupDataFile);

        if (!dataExists && !backupExists) // Vault is not created, create new vault
        {
            vaultData = new VaultData();
            SaveVault(vaultData);
            return;
        }

        try // Load vault
        {
            var content = File.ReadAllText(Constants.ApplicationDataFile);
            vaultData = JsonSerializer.Deserialize<VaultData>(content)
                ?? throw new Exception("Failed to laod vault");

            return;
        }
        catch
        { // Failed to load, using backup
            if (!backupExists)
                throw new Exception("Failed to load vault, backup is missing");

            var backupContent = File.ReadAllText(Constants.ApplicationBackupDataFile);
            vaultData = JsonSerializer.Deserialize<VaultData>(backupContent)
                ?? throw new Exception("Failed to load backup vault");

            return;
        }
    }

    public void TryLoadSettings(out VaultSettings vaultSettings)
    {
        try
        {
            vaultSettings = JsonSerializer.Deserialize<VaultSettings>(File.ReadAllText(Constants.ApplicationSettingsFile))!;
            return;
        }
        catch (FileNotFoundException)
        {
            var settings = new VaultSettings();
            var jsonString = JsonSerializer.Serialize(settings, _options);
            File.WriteAllText(Constants.ApplicationSettingsFile, jsonString);
        }

        vaultSettings = new VaultSettings();
    }

    public void SaveVault(VaultData data)
    {
        lock (data.Lock)
        {
            var jsonString = JsonSerializer.Serialize(data, _options);
            FileUtils.WriteFileSafe(Constants.ApplicationDataFile, jsonString, Constants.ApplicationBackupDataFile);
        }
    }
}
