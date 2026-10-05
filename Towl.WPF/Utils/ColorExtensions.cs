using System.Windows.Media;

namespace Towl.WPF.Utils;

public static class ColorExtensions
{
    public static System.Windows.Media.Color FromDrawing(this System.Drawing.Color color)
        => System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);

    public static SolidColorBrush BrushFromDrawing(this System.Drawing.Color color) =>
        new()
        {
            Color = color.FromDrawing()
        };
}
