using System.Drawing;

namespace KLG.Drawing.ImageSavers;

public class BasicBmpImageSaver : IImageSaver
{
    public async Task SaveImageFile(string path, Color[,] colors)
    {
        await File.WriteAllBytesAsync(path, Bmp.SaveBmp(colors, false));
    }

    public async Task SaveImageFileParallel(string path, Color[,] colors)
    {
        await File.WriteAllBytesAsync(path, Bmp.SaveBmp(colors, true));
    }
}
