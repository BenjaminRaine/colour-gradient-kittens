using System.Drawing.Imaging;
using System.Drawing;

namespace CGKUpdated.Models.Filters
{
    public class NearestNeighbour : Filter
    {
        public NearestNeighbour() { }

        override public void ApplyFilter(Bitmap original)
        {
            Rectangle rect = new Rectangle(0, 0, original.Width, original.Height);
            BitmapData img = original.LockBits(rect, ImageLockMode.ReadWrite, original.PixelFormat);

            IntPtr ptr = img.Scan0;

            int size = Math.Abs(img.Stride) * img.Height;
            byte[] rgbValues = new byte[size];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, size);

            for (int i = 0; i < rgbValues.Length - 6; i += 3)
            {
                int min = i + 3;
                double minDist = ColourDist(rgbValues, i, min);
                for (int j = i + 6; j < rgbValues.Length - 3; j += 3)
                {
                    double currDist = ColourDist(rgbValues, i, j);
                    if (currDist < minDist)
                    {
                        min = j;
                        minDist = currDist;
                    }
                }
                SwapRGB(rgbValues, i + 3, min);
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, size);

            original.UnlockBits(img);
        }

        private static void SwapRGB(byte[] rgb, int i, int j)
        {
            byte tmpr = rgb[i];
            byte tmpg = rgb[i + 1];
            byte tmpb = rgb[i + 2];
            rgb[i] = rgb[j];
            rgb[i + 1] = rgb[j + 1];
            rgb[i + 2] = rgb[j + 2];
            rgb[j] = tmpr;
            rgb[j + 1] = tmpg;
            rgb[j + 2] = tmpb;
        }

        private static double ColourDist(byte[] rgb, int i, int j)
        {
            int rDist = rgb[i] - rgb[j];
            int gDist = rgb[i + 1] - rgb[j + 1];
            int bDist = rgb[i + 2] - rgb[j + 2];
            return Math.Sqrt(rDist * rDist + gDist * gDist + bDist * bDist);
        }
    }
}
