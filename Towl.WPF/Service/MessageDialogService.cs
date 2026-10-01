using System.Windows;
using Towl.Core.Services;

namespace Towl.WPF.Service;

public class MessageDialogService : IMessageDialogService
{
    public void ShowError(string message, string title = "Error")
    {
        System.Windows.Application.Current?.Dispatcher.Invoke(() =>
        {
            System.Windows.MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        });
    }
}
