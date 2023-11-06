using System.Drawing.Imaging;
using System.Drawing;

namespace CGKUpdated.Models.Filters
{
    public class RGBEven : Filter
    {
        public RGBEven() { }
        override public void ApplyFilter(Bitmap original)
        {
            Rectangle rect = new Rectangle(0, 0, original.Width, original.Height);
            BitmapData img = original.LockBits(rect, ImageLockMode.ReadWrite, original.PixelFormat);

            IntPtr ptr = img.Scan0;

            int size = Math.Abs(img.Stride) * img.Height;
            byte[] rgbValues = new byte[size];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, size);

            long rAvg = 0;
            long gAvg = 0;
            long bAvg = 0;
            int lenPerColour = rgbValues.Length / 3;

            for (int i = 0; i < rgbValues.Length - 2; i += 3)
            {
                rAvg += rgbValues[i];
                gAvg += rgbValues[i + 1];
                bAvg += rgbValues[i + 2];
            }

            rAvg /= lenPerColour;
            gAvg /= lenPerColour;
            bAvg /= lenPerColour;

            int avg = (int)(rAvg + gAvg + bAvg) / 3;

            int rDif = (int)rAvg - avg;
            int gDif = (int)gAvg - avg;
            int bDif = (int)bAvg - avg;

            for (int i = 0; i < rgbValues.Length - 2; i += 3)
            {
                rgbValues[i] = (byte)Math.Min(255, Math.Max(rgbValues[i] - rDif, 0));
                rgbValues[i + 1] = (byte)Math.Min(255, Math.Max(rgbValues[i + 1] - gDif, 0));
                rgbValues[i + 2] = (byte)Math.Min(255, Math.Max(rgbValues[i + 2] - bDif, 0));
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, size);

            original.UnlockBits(img);
        }
    }
}
