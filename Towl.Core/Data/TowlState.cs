using Towl.Core.Data.Session;
using Towl.Core.Data.Settings;

namespace Towl.Core.Data;

public class TowlState
{
    public required TowlSessionData Data { get; set; } = new();
    public required TowlSettings Settings { get; set; } = new();

    public required bool CursorMoved { get; set; } = true;
}
