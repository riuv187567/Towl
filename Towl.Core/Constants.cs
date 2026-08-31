using System.Drawing;

namespace Towl.Core;

public static class Constants
{
    public const int CycleSeconds = 1;
    public const int CursorMoveTimeout = 5;

    public static Color ActiveColor = Color.PaleGreen;
    public static Color NotActiveColor = Color.PaleVioletRed;
    public static Color NotFoundColor = Color.LightGray;

    public const string ApplicationDataFile = "Data.json";
    public const string ApplicationSettingsFile = "Settings.json";

    public const string NoProcessDisplayedText = "No process displayed";
}
