using System.Drawing;

namespace KLG.Drawing;

internal static class Extension
{
    internal static Color[] Flatten(this Color[,] colors, bool parallel)
    {
        Color[] flatColors = new Color[colors.Length];
        int width = colors.GetLength(0);
        int height = colors.GetLength(1);
        if (parallel)
        {
            Parallel.For(0, width, x =>
            {
                for (int y = 0; y < height; y++)
                {
                    flatColors[y * width + x] = colors[x, y];
                }
            });
        }
        else
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    flatColors[y * width + x] = colors[x, y];
                }
            }
        }
        return flatColors;
    }
}
