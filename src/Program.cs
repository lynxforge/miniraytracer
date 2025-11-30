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

        static Vec3f Reflect(Vec3f I, Vec3f N)
        {
            return I - N * 2f * (I * N);
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

        static Vec3f CastRay(Vec3f origin, Vec3f dir, List<Sphere> spheres, List<Light> lights)
        {
            Vec3f point = new Vec3f();
            Vec3f N = new Vec3f();
            Material material = new Material();

            if(!SceneIntersect(origin, dir, spheres, ref point, ref N, ref material))
            {
                return new Vec3f(0.2f, 0.7f, 0.8f);
            }

            float diffuse_light_intensity = 0;
            float specular_light_intensity = 0;

            for (int i = 0; i < lights.Count; i++)
            {
                //Diffuse Component
                Vec3f light_dir = (lights[i].position - point).Normalize();

                float light_dist = (lights[i].position - point).Norm();
                Vec3f shadow_origin = (light_dir * N < 0) 
                    ? point - N * 1e-3f 
                    : point + N * 1e-3f;

                Vec3f shadow_hit = new Vec3f();
                Vec3f shadow_N = new Vec3f();
                Material tempMaterial = new Material();

                if(SceneIntersect(shadow_origin, light_dir, spheres, ref shadow_hit, ref shadow_N, ref tempMaterial))
                {
                    float dist_to_blocker = (shadow_hit - shadow_origin).Norm();
                    if(dist_to_blocker < light_dist)
                    {
                        continue;
                    }
                }
                diffuse_light_intensity += lights[i].intensity * Math.Max(0f, light_dir * N);

                //Specular Component
                Vec3f light_ref = Reflect(-light_dir, N);
                float spec_factor = (float)Math.Pow(Math.Max(0f, -light_ref * dir), material.specular_exponent);
                specular_light_intensity += spec_factor * lights[i].intensity;
            }

            Vec3f diffuse_part = material.diffuse_color * diffuse_light_intensity * material.albedo.x;
            Vec3f specular_part = new Vec3f(1f, 1f, 1f) * specular_light_intensity * material.albedo.y;
            
            return diffuse_part + specular_part;
        }

        static void Render(List<Sphere> spheres, List<Light> lights)
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

                    framebuffer[i + j * width] = CastRay(new Vec3f(0, 0, 0), dir, spheres, lights);
                }
            }

            using (Bitmap bitmap = new Bitmap(width, height))
            {
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        Vec3f color = framebuffer[i + j * width];

                        float max = Math.Max(color.x, Math.Max(color.y, color.z));
                        if(max > 1)
                        {
                            color = color * (1f / max);
                        }
                       
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
            Material ivory = new Material(new Vec2f(0.6f, 0.3f), new Vec3f(0.4f, 0.4f, 0.3f), 50f);
            Material red_rubber = new Material(new Vec2f(0.9f, 0.1f), new Vec3f(0.3f, 0.1f, 0.1f), 10f);
            Material mirror = new Material(new Vec2f(0.2f, 0.8f), new Vec3f(1.0f, 1.0f, 1.0f), 1425f);
            Material charcoal = new Material(new Vec2f(0.95f, 0.05f), new Vec3f(0.15f, 0.15f, 0.15f), 10f);
            Material amethyst = new Material(new Vec2f(0.6f, 0.4f), new Vec3f(0.6f, 0.5f, 0.95f), 200f);

            List<Sphere> spheres = new List<Sphere>
            {
                new Sphere(new Vec3f(-4.8f, -1.8f, -16), 5.5f, mirror),
                new Sphere(new Vec3f(8, 8, -16), 2.9f, red_rubber),
                new Sphere(new Vec3f(-4, 6, -23), 2.8f, ivory),
                new Sphere(new Vec3f(3, 3, -20), 4.8f, charcoal),
                new Sphere(new Vec3f(4, -4, -24), 3.8f, amethyst),
            };

            List<Light> lights = new List<Light>
            {
                new Light(new Vec3f(-20, 20, 20), 1.5f),
                new Light(new Vec3f(30, 50, -25), 1.8f),
                new Light(new Vec3f(30, 20, 30), 1.7f),
            };

            Render(spheres, lights);
        }
    } 
}