using Microsoft.Extensions.Hosting;
using Towl.Core.Data;

namespace Towl.Core.Services;

public class VaultBackupService(VaultManager state, IMessageDialogService errorDialogService) : BackgroundService
{
    private readonly VaultManager _state = state;
    private readonly IMessageDialogService _errorDialogService = errorDialogService;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var time = new PeriodicTimer(TimeSpan.FromSeconds(30));

        while (await time.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                _state._storage.SaveData(_state.Current.Data);
            }
            catch
            {
                _errorDialogService.ShowError("Failed to perform vault backup");
            }
        }
    }
}
