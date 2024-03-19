using System.Drawing;

namespace KLG.Drawing;

public interface IImageSaver
{
    Task SaveImageFile(string path, Color[,] colors);
    Task SaveImageFileParallel(string path, Color[,] colors);
}
