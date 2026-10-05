using System.Drawing;

namespace Towl.Core;

public static class MainColors
{
    public readonly static Color Green = Color.FromArgb(255, 48, 209, 89); // #30D159
    public readonly static Color Red = Color.FromArgb(255, 255, 69, 58); // #FF453A
}

public static class Constants
{
    public const int CycleSeconds = 1;
    public const int CursorMoveTimeout = 10;

    public readonly static Color ActiveColor = MainColors.Green;
    public readonly static Color NotActiveColor = MainColors.Red;
    public readonly static Color NotFoundColor = Color.LightGray;

    public const string VaultFilename = "Vault.json";
    public const string VaultBackupFilename = "VaultBackup.json";

    public const string NoProcessDisplayedText = "No process displayed";
}
