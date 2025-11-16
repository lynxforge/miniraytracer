using System;
using System.IO;
using System.Numerics;


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

            using (FileStream fs = new FileStream("../../../output/out.ppm", FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                string header = $"P6\n{width} {height}\n255\n";
                byte[] headerBytes = System.Text.Encoding.ASCII.GetBytes(header);
                writer.Write(headerBytes);

                for (int i = 0; i < height * width; i++)
                {
                    Vector3 color = framebuffer[i];
                    writer.Write((byte)(255 * Clamp(color.X, 0f, 1f)));
                    writer.Write((byte)(255 * Clamp(color.Y, 0f, 1f)));
                    writer.Write((byte)(255 * Clamp(color.Z, 0f, 1f)));
                }
            }
            Console.WriteLine("Image saved!");
        }

        static void Main()
        {
            Render();
        }
    } 
}