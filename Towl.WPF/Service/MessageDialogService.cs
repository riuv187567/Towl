using System.Windows;
using Towl.Core.Services;

namespace Towl.WPF.Service;

public class MessageDialogService : IMessageDialogService
{
    public void ShowErrorMessage(string message, string title = "Error")
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher is null)
            return;

        void Show() => System.Windows.MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);

        if (dispatcher.CheckAccess())
            Show();
        else
            dispatcher.BeginInvoke(Show);
    }
}
