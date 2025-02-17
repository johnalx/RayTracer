

using System.IO;
using System.Runtime.InteropServices;

namespace JA.RayTracer.Graphics
{
    internal class Image
    {
        private RGBColor[] data;
        public int Width { get; private set; }
        public int Height { get; private set; }

        #region Win API
        [StructLayout(LayoutKind.Sequential)]
        private struct BITMAPINFOHEADER
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public ushort biBitCount;
            public uint biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        private struct BITMAPFILEHEADER
        {
            public ushort bfType;
            public uint bfSize;
            public ushort bfReserved1;
            public ushort bfReserved2;
            public uint bfOffBits;
        } 
        #endregion

        public Image(int width, int height)
        {
            Width = width;
            Height = height;
            data = new RGBColor[(width * height)];
        }

        public RGBColor this[int index]
        {
            get { return data[index]; }
            set { data[index] = value; }
        }
        public byte[] GetBytes()
        {
            var ms = new MemoryStream();
            var writer = new BinaryWriter(ms);
            foreach (var color in data)
            {
                writer.Write(color.B);
                writer.Write(color.G);
                writer.Write(color.R);
                writer.Write(color.A);
            }
            writer.Close();
            return ms.ToArray();
        }
        public System.Drawing.Bitmap Export()
        {
            var pixelArray = GetBytes();
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // Create a BitmapData and lock all pixels to be written 
            System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(
                                new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                                System.Drawing.Imaging.ImageLockMode.WriteOnly, bmp.PixelFormat);
            // Copy the data from the byte array into BitmapData.Scan0
            Marshal.Copy(pixelArray, 0, bmpData.Scan0, pixelArray.Length);

            // Unlock the pixels
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        public void Save(string fileName)
        {
            var infoHeaderSize = Marshal.SizeOf(typeof(BITMAPINFOHEADER));
            var fileHeaderSize = Marshal.SizeOf(typeof(BITMAPFILEHEADER));
            var offBits = infoHeaderSize + fileHeaderSize;

            BITMAPINFOHEADER infoHeader = new BITMAPINFOHEADER
            {
                biSize = (uint)infoHeaderSize,
                biBitCount = 32,
                biClrImportant = 0,
                biClrUsed = 0,
                biCompression = 0,
                biHeight = -Height,
                biWidth = Width,
                biPlanes = 1,
                biSizeImage = (uint)(Width * Height * 4)
            };

            BITMAPFILEHEADER fileHeader = new BITMAPFILEHEADER
            {
                bfType = 'B' + ('M' << 8),
                bfOffBits = (uint)offBits,
                bfSize = (uint)(offBits + infoHeader.biSizeImage)
            };

            using (var writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                writer.Write(ImageHelpers.GetBytes(fileHeader));
                writer.Write(ImageHelpers.GetBytes(infoHeader));
                var buffer = GetBytes();
                writer.Write(buffer);
                //foreach (var color in data)
                //{
                //    writer.Write(color.B);
                //    writer.Write(color.G);
                //    writer.Write(color.R);
                //    writer.Write(color.A);
                //}
            }
        }
    }

    internal static class ImageHelpers
    {
        public static byte[] GetBytes<T>(this T data)
        {
            var length = Marshal.SizeOf(data);
            var ptr = Marshal.AllocHGlobal(length);
            var result = new byte[length];
            Marshal.StructureToPtr(data, ptr, true);
            Marshal.Copy(ptr, result, 0, length);
            Marshal.FreeHGlobal(ptr);
            return result;
        }
    }
}