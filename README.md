# MiniRAytracer in C#

A C# implementation of the [tinyraytracer](https://github.com/ssloy/tinyraytracer) tutorial by `Dimitry V. Sokolov (ssloy)`

## About

This project is a learning exercise to understand raytracing fundamentals by converting the original C++ code to C#.

## Current Progress

- PPM image saved.
- Ray sphere intersection vec3f.
- JPEG image saved.
- custom 3D math vector (Vec2f, Vec3f, Vec3i, Vec4f).
- operator overloading of vector operations (dot product, scalar multiplication).
- single sphere rendered.
- more sphere added with different materials
- lighting and shadow to the spheres

## How to Run

```
dotnet build
dotnet run
```

The program will generate an `out.ppm` file in the project directory.

## Credits 

Original project: [ssloy/tinyraytracer](https://github.com/ssloy/tinyraytracer)

Licensed under [DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE](https://en.wikipedia.org/wiki/WTFPL)