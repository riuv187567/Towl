using Towl.Core.Data.Session;
using Towl.Core.Data.Settings;

namespace Towl.Core.Data.Store;

public interface IDataStore
{
    public VaultSettings LoadSettings();
    public VaultData LoadData();
    public void SaveData(VaultData data);
}