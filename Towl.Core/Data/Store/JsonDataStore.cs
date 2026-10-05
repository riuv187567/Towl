using System.Text.Json;
using Towl.Core.Data.Session;
using Towl.Core.Data.Settings;
using Towl.Core.Utils;

namespace Towl.Core.Data.Store;

public class JsonDataStore : IDataStore
{
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public void TryLoadVault(out VaultData vaultData, out VaultSettings vaultSettings)
    {
        var dataExists = File.Exists(Constants.VaultFilename);
        var backupExists = File.Exists(Constants.VaultBackupFilename);

        if (!dataExists && !backupExists) // Vault is not created, create new vault
        {
            vaultData = new VaultData();
            vaultSettings = new VaultSettings();
            SaveVault(vaultData, vaultSettings);
            return;
        }

        try // Load vault
        {
            var content = File.ReadAllText(Constants.VaultFilename);
            var vaultJson = JsonSerializer.Deserialize<VaultJson>(content)
                ?? throw new Exception("Failed to laod vault");

            vaultData = vaultJson.Data;
            vaultSettings = vaultJson.Settings;

            return;
        }
        catch
        { // Failed to load, using backup
            if (!backupExists)
                throw new Exception("Failed to load vault, backup is missing");

            var backupContent = File.ReadAllText(Constants.VaultBackupFilename);
            var vaultJson = JsonSerializer.Deserialize<VaultJson>(backupContent)
                ?? throw new Exception("Failed to load backup vault");

            vaultData = vaultJson.Data;
            vaultSettings = vaultJson.Settings;

            return;
        }
    }

    public void SaveVault(VaultData data, VaultSettings vaultSettings)
    {
        lock (data.Lock)
        {
            var vaultJson = new VaultJson()
            {
                Data = data,
                Settings = vaultSettings
            };

            var jsonString = JsonSerializer.Serialize(vaultJson, _options);
            FileUtils.WriteFileSafe(Constants.VaultFilename, jsonString, Constants.VaultBackupFilename);
        }
    }
}
