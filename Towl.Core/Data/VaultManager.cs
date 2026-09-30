using Towl.Core.Data.Store;

namespace Towl.Core.Data;

public class VaultManager
{
    public Vault Current;

    public readonly IDataStore _storage;

    public VaultManager()
    {
        _storage = new JsonDataStore();

        Current = new Vault()
        {
            Data = _storage.LoadData(),
            Settings = _storage.LoadSettings()
        };
    }
}