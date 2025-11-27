using System;
using System.IO;
using System.Numerics;
using System.Drawing;
using System.Drawing.Imaging;


namespace raytracer
{
    class Program
    {
        static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        static void Render()
        {
            const int width = 1024;
            const int height = 768;
            Vector3[] framebuffer = new Vector3[width * height];

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    framebuffer[i + j * width] = new Vector3(
                        j / (float)height,
                        i / (float)width,
                        0);
                }
            }

            using (Bitmap bitmap = new Bitmap(width, height))
            {
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        Vector3 color = framebuffer[i + j * width];
                        int r = (int)(255 * Clamp(color.X, 0f, 1f));
                        int g = (int)(255 * Clamp(color.Y, 0f, 1f));
                        int b = (int)(255 * Clamp(color.Z, 0f, 1f));
                        bitmap.SetPixel(i, j, Color.FromArgb(r, g, b));
                    }
                }

                bitmap.Save("../../../output/out.jpg", ImageFormat.Jpeg);
            }

            Console.WriteLine(("Image Saved!"));
        }
        static void Main()
        {
            Render();
        }
    } 
}