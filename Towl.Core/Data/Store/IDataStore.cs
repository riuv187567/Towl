using Towl.Core.Data.Session;
using Towl.Core.Data.Settings;

namespace Towl.Core.Data.Store;

public interface IDataStore
{
    public void TryLoadData(out VaultData vaultData);
    public void TryLoadSettings(out VaultSettings vaultSettings);

    public void SaveVault(VaultData data);
}