using System;
using System.IO;
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

        static Vec3f CastRay(Vec3f origin, Vec3f dir, Sphere sphere)
        {
            float sphere_dist = float.MaxValue;
            if (!sphere.RayIntersect(origin, dir, ref sphere_dist))
            {
                return new Vec3f(0.2f, 0.7f, 0.9f);
            }
            return new Vec3f(0.4f, 0.4f, 0.3f);
        }

        static void Render(Sphere sphere)
        {
            const int width = 1024;
            const int height = 768;
            const float fov = (float)(Math.PI / 2.0);
            Vec3f[] framebuffer = new Vec3f[width * height];

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    float x = (2 * (i + 0.5f) / (float)width - 1) * (float)Math.Tan(fov / 2.0) * width / (float)height;
                    float y = -(2 * (j + 0.5f) / (float)height - 1) * (float)Math.Tan(fov / 2.0);
                    Vec3f dir = new Vec3f(x, y, -1);
                    dir = dir.Normalize();

                    framebuffer[i + j * width] = CastRay(new Vec3f(0, 0, 0), dir, sphere);
                }
            }

            using (Bitmap bitmap = new Bitmap(width, height))
            {
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        Vec3f color = framebuffer[i + j * width];
                        int r = (int)(255 * Clamp(color.x, 0f, 1f));
                        int g = (int)(255 * Clamp(color.y, 0f, 1f));
                        int b = (int)(255 * Clamp(color.z, 0f, 1f));
                        bitmap.SetPixel(i, j, Color.FromArgb(r, g, b));
                    }
                }

                bitmap.Save("../../../output/out.jpg", ImageFormat.Jpeg);
            }

            Console.WriteLine(("Image Saved!"));
        }
        static void Main()
        {
            Sphere sphere = new Sphere(new Vec3f(3, 0, -16), 4);
            Render(sphere);
        }
    } 
}