using Towl.Core.Data.Store;
using Towl.Core.Services;

namespace Towl.Core.Data;

public class VaultManager
{
    public readonly IDataStore _storage;
    private readonly IMessageDialogService _errorDialogService;

    public Vault Current;

    public VaultManager(IMessageDialogService errorDialogService)
    {
        _storage = new JsonDataStore();
        _errorDialogService = errorDialogService;

        Current = new Vault();

        try
        {
            _storage.TryLoadVault(out var data, out var settings);
            Current.Data = data;
            Current.Settings = settings;
        }
        catch
        {
            _errorDialogService.ShowErrorMessage("Failed to load vault, saving is disabled");
            Current.SavingEnabled = false;
        }
    }
}