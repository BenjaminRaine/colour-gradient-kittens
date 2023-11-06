using System.Drawing;
using System.Drawing.Imaging;

namespace CGKUpdated.Models.Filters
{
    public class RGBRShift : Filter 
    {
        public RGBRShift() { }

        override public void ApplyFilter(Bitmap original)
        {
            Rectangle rect = new Rectangle(0, 0, original.Width, original.Height);
            BitmapData img = original.LockBits(rect, ImageLockMode.ReadWrite, original.PixelFormat);

            IntPtr ptr = img.Scan0;

            int size = Math.Abs(img.Stride) * img.Height;
            byte[] rgbValues = new byte[size];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, size);

            for (int i = 0; i < rgbValues.Length - 2; i += 3)
            {
                byte tempr = rgbValues[i];
                rgbValues[i] = rgbValues[i + 1];
                rgbValues[i + 1] = rgbValues[i + 2];
                rgbValues[i + 2] = tempr;
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, size);

            original.UnlockBits(img);
        }
    }
}
