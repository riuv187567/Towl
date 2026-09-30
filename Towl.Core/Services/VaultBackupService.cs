using Microsoft.Extensions.Hosting;
using Towl.Core.Data;

namespace Towl.Core.Services;

public class VaultBackupService(VaultManager state) : BackgroundService
{
    private readonly VaultManager _state = state;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var time = new PeriodicTimer(TimeSpan.FromSeconds(30));

        while (await time.WaitForNextTickAsync(stoppingToken))
            _state._storage.SaveData(_state.Current.Data);
    }
}
