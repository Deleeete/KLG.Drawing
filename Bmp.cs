using System.Drawing;

namespace KLG.Drawing;

public static class Bmp
{
    private static readonly byte[] BmpHeaderReserved = [0, 0, 0, 0];

    private const int Size_BmpHeader = 14;
    private const int Size_DibHeader = 40;
    private const int Offset_PixelOffset = Size_BmpHeader + Size_DibHeader;
    private const ushort ColorPlanes = 1;
    private const ushort BitsPerPixel = 24;
    private const int CompressionMethod = 0;
    private const int ResolutionH = 0xec4;
    private const int ResolutionV = 0xec4;

    public static byte[] SaveBmp(int width, int height, Color[] colors)
    {
        byte[] pixelData = ToPixelData(colors);
        int fileSize = Size_BmpHeader + Size_DibHeader + pixelData.Length;
        List<byte> data = [];
        data.Capacity = fileSize;
        // BMP header
        data.Add((byte)'B');
        data.Add((byte)'M');
        data.AddRange(BitConverter.GetBytes(fileSize));
        data.AddRange(BmpHeaderReserved);
        data.AddRange(BitConverter.GetBytes(Offset_PixelOffset));
        // DIB
        // the size of this header, in bytes
        data.AddRange(BitConverter.GetBytes(Size_DibHeader));
        data.AddRange(BitConverter.GetBytes(width));
        data.AddRange(BitConverter.GetBytes(-height));
        //	the number of color planes (must be 1)
        data.AddRange(BitConverter.GetBytes(ColorPlanes));
        // the number of bits per pixel, which is the color depth of the image. Typical values are 1, 4, 8, 16, 24 and 32.
        data.AddRange(BitConverter.GetBytes(BitsPerPixel));
        // the compression method being used. See the next table for a list of possible values
        data.AddRange(BitConverter.GetBytes(CompressionMethod));
        // the image size. This is the size of the raw bitmap data; a dummy 0 can be given for BI_RGB bitmaps.
        data.AddRange(BitConverter.GetBytes(0));
        // the horizontal resolution of the image. (pixel per metre, signed integer)
        data.AddRange(BitConverter.GetBytes(ResolutionH));
        // the vertical resolution of the image. (pixel per metre, signed integer)
        data.AddRange(BitConverter.GetBytes(ResolutionV));
        // the number of colors in the color palette, or 0 to default to 2n
        data.AddRange(BitConverter.GetBytes(0));
        // the number of important colors used, or 0 when every color is important; generally ignored
        data.AddRange(BitConverter.GetBytes(0));
        data.AddRange(pixelData);
        return [.. data];
    }

    public static byte[] SaveBmp(Color[,] colors, bool parallel)
    {
        int width = colors.GetLength(0);
        int height = colors.GetLength(1);
        return SaveBmp(width, height, colors.Flatten(parallel));
    }

    private static byte[] ToPixelData(Color[] colors)
    {
        int dataLength = colors.Length * 3;
        if (dataLength % 4 != 0)
            dataLength += 4 - (dataLength % 4);
        byte[] bin = new byte[dataLength];
        bin.Initialize();
        Parallel.For(0, colors.Length, i =>
        {
            bin[i * 3] = colors[i].B;
            bin[i * 3 + 1] = colors[i].G;
            bin[i * 3 + 2] = colors[i].R;
        });
        return bin;
    }
}
