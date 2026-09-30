using Towl.Core.Data.Session;
using Towl.Core.Data.Settings;

namespace Towl.Core.Data.Store;

public interface IDataStore
{
    public VaultSettings LoadSettings();
    public VaultSessionData LoadData();
    public void SaveData(VaultSessionData data);
}