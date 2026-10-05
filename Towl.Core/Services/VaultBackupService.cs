using Microsoft.Extensions.Hosting;
using Towl.Core.Data;

namespace Towl.Core.Services;

public class VaultBackupService(VaultManager state, IMessageDialogService errorDialogService) : BackgroundService
{
    private readonly VaultManager _state = state;
    private readonly IMessageDialogService _errorDialogService = errorDialogService;
    private bool _saveFailed = false;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var time = new PeriodicTimer(TimeSpan.FromSeconds(30));

        while (await time.WaitForNextTickAsync(stoppingToken))
        {
            if (!_state.Current.SavingEnabled)
                continue;

            try
            {
                _state._storage.SaveVault(_state.Current.Data, _state.Current.Settings);
                _saveFailed = false;
            }
            catch
            {
                if (!_saveFailed) // Error message shown only once
                    _errorDialogService.ShowErrorMessage("Failed to perform vault backup");

                _saveFailed = true;
            }
        }
    }
}
