using Towl.Core.Data.Session;
using Towl.Core.Data.Settings;

namespace Towl.Core.Data;

public class Vault
{
    public VaultData Data { get; set; } = new();
    public VaultSettings Settings { get; set; } = new();

    public bool SavingEnabled { get; set; } = true;
}
