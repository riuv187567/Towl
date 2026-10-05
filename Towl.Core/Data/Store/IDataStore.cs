using Towl.Core.Data.Session;
using Towl.Core.Data.Settings;

namespace Towl.Core.Data.Store;

public interface IDataStore
{
    public void TryLoadVault(out VaultData vaultData, out VaultSettings vaultSettings);
    public void SaveVault(VaultData data, VaultSettings vaultSettings);
}