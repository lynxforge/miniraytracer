using System;
using System.Diagnostics;

namespace raytracer
{
    public struct Vec<T> where T : struct
    {
        private readonly T[] data;
        public Vec(int dimension)
        {
            data = new T[dimension];
        }

        public T this[int i]
        {
            get
            {
                Debug.Assert(i < data.Length);
                return data[i];
            }
            set
            {
                Debug.Assert(i < data.Length);
                data[i] = value;
            }
        }
    }

    public struct Vec2f
    {
        public float x, y;

        public Vec2f(float X = 0, float Y = 0)
        {
            x = X;
            y = Y;
        }
        public float this[int i]
        {
            get
            {
                Debug.Assert(i < 2);
                return i <= 0 ? x : y;
            }
            set
            {
                Debug.Assert(i < 2);
                if (i <= 0) x = value;
                else y = value;
            }
        }
    }

    public struct Vec3f
    {
        public float x, y, z;
        public Vec3f(float X = 0, float Y = 0, float Z = 0)
        {
            x = X;
            y = Y;
            z = Z;
        }
        public float this[int i]
        {
            get
            {
                Debug.Assert(i < 3);
                return i <= 0 ? x : (i == 1 ? y : z);
            }
            set
            {
                Debug.Assert(i < 3);
                if (i <= 0) x = value;
                else if (i == 1) y = value;
                else z = value;
            }
        }

        public float Norm()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        public Vec3f Normalize(float l = 1)
        {
            float norm = Norm();
            x *= l / norm;
            y *= l / norm;
            z *= l / norm;
            return this;
        }

        //Dot Product
        public static float operator *(Vec3f lhs, Vec3f rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        //Scalar Multiplication
        public static Vec3f operator *(Vec3f lhs, float rhs)
        {
            return new Vec3f(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
        }
        public static Vec3f operator *(float lhs, Vec3f rhs)
        {
            return new Vec3f(rhs.x * lhs, rhs.y * lhs, rhs.z * lhs);
        }

        //Addition
        public static Vec3f operator +(Vec3f lhs, Vec3f rhs)
        {
            return new Vec3f(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }

        //Subtraction
        public static Vec3f operator -(Vec3f lhs, Vec3f rhs)
        {
            return new Vec3f(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        //Negation
        public static Vec3f operator -(Vec3f lhs)
        {
            return new Vec3f(-lhs.x, -lhs.y, -lhs.z);
        }

        public override string ToString()
        {
            return $"{x} {y} {z}";
        }
    }

    //Vec3i - 3D vector
    public struct Vec3i
    {
        public int x, y, z;

        public Vec3i(int X = 0, int Y = 0, int Z = 0)
        {
            x = X;
            y = Y;
            z = Z;
        }

        public int this[int i]
        {
            get
            {
                Debug.Assert(i < 3);
                return i <= 0 ? x : (i == 1 ? y : z);
            }
            set
            {
                Debug.Assert(i < 3);
                if (i <= 0) x = value;
                else if (i == 1) y = value;
                else z = value;
            }
        }

        //Dot product 
        public static int operator *(Vec3i lhs, Vec3i rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        //Scalar multiplication
        public static Vec3i operator *(Vec3i lhs, int rhs)
        {
            return new Vec3i(lhs.x * rhs, lhs.y * rhs, lhs.z * rhs);
        }

        //Addition
        public static Vec3i operator +(Vec3i lhs, Vec3i rhs)
        {
            return new Vec3i(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }

        //Subtraction
        public static Vec3i operator -(Vec3i lhs, Vec3i rhs)
        {
            return new Vec3i(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        //Negation
        public static Vec3i operator -(Vec3i lhs)
        {
            return new Vec3i(-lhs.x, -lhs.y, -lhs.z);
        }
    }

    public struct Vec4f
    {
        public float x, y, z, w;

        public Vec4f(float X = 0, float Y = 0, float Z = 0, float W = 0)
        {
            x = X;
            y = Y;
            z = Z;
            w = W;
        }
        public float this[int i]
        {
            get
            {
                Debug.Assert(i < 4);
                return i <= 0 ? x : (i == 1 ? y : (i == 2 ? z : w));
            }
            set
            {
                Debug.Assert(i < 4);
                if (i <= 0) x = value;
                else if (i == 1) y = value;
                else if (i == 2) z = value;
                else w = value;
            }
        }
    }

    public static class VectorExtensions
    {
        public static Vec3f Cross(Vec3f v1, Vec3f v2)
        {
            return new Vec3f(
                v1.y * v2.z - v1.z * v2.y,
                v1.z * v2.x - v1.x * v2.z,
                v1.x * v2.y - v1.y * v2.x
            );
        }

        public static Vec3i Cross(Vec3i v1, Vec3i v2)
        {
            return new Vec3i(
                v1.y * v2.z - v1.z * v2.y,
                v1.z * v2.x - v1.x * v2.z,
                v1.x * v2.y - v1.y * v2.x
            );
        }
    }

    public struct Material
    {
        public float refractive_index;
        public Vec4f albedo; //(diffuse, specular, reflective, refractive)
        public Vec3f diffuse_color;
        public float specular_exponent; //Shininess factor

        public Material(float refr_idx, Vec4f a, Vec3f color, float spec)
        {
            refractive_index = refr_idx;
            albedo = a;
            diffuse_color = color;
            specular_exponent = spec; 
        }

        //Default constructor for initialization
        public Material(Vec3f color) : this(1.0f, new Vec4f(1, 0, 0, 0), color, 0) { }
    }

    public struct Light
    {
        public Vec3f position;
        public float intensity;

        public Light(Vec3f p, float i)
        {
            position = p;
            intensity = i;
        }
    }

    public struct Sphere
    {
        public Vec3f center;
        public float radius;
        public Material material;

        public Sphere(Vec3f c, float r, Material m)
        {
            center = c;
            radius = r;
            material = m;
        }

        public bool RayIntersect(Vec3f origin, Vec3f dir, ref float t0)
        {
            Vec3f L = center - origin;
            float tca = L * dir;
            float d2 = (L * L) - (tca * tca);

            if(d2 > radius * radius)
            {
                return false;
            }

            float thc = (float)Math.Sqrt(radius * radius - d2);
            t0 = tca - thc;
            float t1 = tca + thc;

            if(t0 < 0) t0 = t1;
            if (t0 < 0) return false;

            return true;
        }
    }
}