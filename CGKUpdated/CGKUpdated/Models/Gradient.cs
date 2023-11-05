using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Drawing;
using System.Drawing.Imaging;

namespace CGKProject.Models
{
    public static class Gradient
    {
        public static void GenGradient(Bitmap original)
        {
            Rectangle rect = new Rectangle(0, 0, original.Width, original.Height);
            BitmapData gradientData = original.LockBits(rect, ImageLockMode.ReadWrite, original.PixelFormat);
            //SortPixels(gradientData);
            //EvenOutPixels(gradientData);
            ColourShiftPixels(gradientData);
            original.UnlockBits(gradientData);
        }

        public static void ColourShiftPixels(BitmapData img)
        {
            IntPtr ptr = img.Scan0;

            int size = Math.Abs(img.Stride) * img.Height;
            byte[] rgbValues = new byte[size];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, size);

            for (int i = 0; i < rgbValues.Length - 2; i += 3)
            {
                byte tempr = rgbValues[i];
                rgbValues[i] = rgbValues[i+1];
                rgbValues[i + 1] = rgbValues[i+2];
                rgbValues[i + 2] = tempr;
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, size);
        }

        private static void EvenOutPixels(BitmapData img)
        {
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
                gAvg += rgbValues[i+1];
                bAvg += rgbValues[i+2];
            }

            rAvg /= lenPerColour;
            gAvg /= lenPerColour;
            bAvg /= lenPerColour;

            int avg = (int) (rAvg + gAvg + bAvg) / 3;

            int rDif = (int) rAvg - avg;
            int gDif = (int) gAvg - avg;
            int bDif = (int) bAvg - avg;

            for (int i = 0; i < rgbValues.Length - 2; i += 3)
            {
                rgbValues[i] = (byte) Math.Min(255, Math.Max(rgbValues[i]-rDif, 0));
                rgbValues[i + 1] = (byte) Math.Min(255, Math.Max(rgbValues[i+1] - gDif, 0));
                rgbValues[i + 2] = (byte) Math.Min(255, Math.Max(rgbValues[i+2] - bDif, 0));
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, size);
        }


        private static void SortPixels(BitmapData img)
        {
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
                SwapRGB(rgbValues, i+3, min);
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, size);
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
