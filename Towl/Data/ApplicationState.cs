namespace Towl.Data;

public class ApplicationState
{
    public required ApplicationSessionData Data { get; set; } = new();
    public required ApplicationSettings Settings { get; set; } = new();

    public required bool CursorMoved { get; set; } = true;
}
