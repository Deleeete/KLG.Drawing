using System.Drawing;

namespace KLG.Drawing;

public interface IImageReader
{
    Color[,] LoadImageFile(string path);
}
