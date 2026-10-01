namespace Towl.Core.Services;

public interface IMessageDialogService
{
    void ShowErrorMessage(string message, string title = "Error");
}
