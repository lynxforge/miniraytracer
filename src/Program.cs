using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;


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

        static bool SceneIntersect(Vec3f origin, Vec3f dir, List<Sphere> spheres, ref Vec3f hit, ref Vec3f N, ref Material material)
        {
            float spheres_dist = float.MaxValue;

            foreach (Sphere sphere in spheres)
            {
                float dist_i = 0;
                if(sphere.RayIntersect(origin, dir, ref dist_i) && dist_i < spheres_dist)
                {
                    spheres_dist = dist_i;
                    hit = origin + dir * dist_i;
                    N = (hit - sphere.center).Normalize();
                    material = sphere.material;
                }
            }
            return spheres_dist < 1000;
        }

        static Vec3f CastRay(Vec3f origin, Vec3f dir, List<Sphere> spheres)
        {
            Vec3f point = new Vec3f();
            Vec3f N = new Vec3f();
            Material material = new Material();

            if(!SceneIntersect(origin, dir, spheres, ref point, ref N, ref material))
            {
                return new Vec3f(0.2f, 0.7f, 0.8f);
            }
            return material.diffuse_color;
        }

        static void Render(List<Sphere> spheres)
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

                    framebuffer[i + j * width] = CastRay(new Vec3f(0, 0, 0), dir, spheres);
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
            Material ivory = new Material(new Vec3f(0.4f, 0.4f, 0.3f));
            Material red_rubber = new Material(new Vec3f(0.3f, 0.1f, 0.1f));
            Material mirror = new Material(new Vec3f(1.0f, 1.0f, 1.0f));
            Material charcoal = new Material(new Vec3f(0.2f, 0.2f, 0.2f));
            Material amethyst = new Material(new Vec3f(0.5f, 0.0f, 1.0f));

            List<Sphere> spheres = new List<Sphere>
            {
                new Sphere(new Vec3f(-4.8f, -1.8f, -16), 5.5f, mirror),
                new Sphere(new Vec3f(8, 8, -16), 2.9f, red_rubber),
                new Sphere(new Vec3f(-4, 6, -23), 2.8f, ivory),
                new Sphere(new Vec3f(3, 3, -20), 4.8f, charcoal),
                new Sphere(new Vec3f(4, -4, -24), 3.8f, amethyst),
            };

            Render(spheres);
        }
    } 
}