using System;
using JA.RayTracer.Geometry;

namespace JA.RayTracer
{
    using Ray = JA.RayTracer.Geometry.Ray;
    using Color = JA.RayTracer.Graphics.Color;

    internal interface ISurface
    {
        SurfaceProperties GetSurfaceProperties(in Vector pos);
    }

    internal interface IThing
    {
        Intersection Intersect(in Ray ray);

        Vector Normal(in Vector pos);

        ISurface Surface { get; set; }
    }


    internal struct SurfaceProperties
    {
        public Color Diffuse;
        public Color Specular;
        public double Reflect;
        public double Roughness;
    }

    internal class ShinySurface : ISurface
    {
        public SurfaceProperties GetSurfaceProperties(in Vector pos)
        {
            return new SurfaceProperties()
            {
                Diffuse = Color.White,
                Specular = Color.Grey,
                Reflect = 0.7,
                Roughness = 250
            };
        }
    }

    internal class CheckerboardSurface : ISurface
    {
        public SurfaceProperties GetSurfaceProperties(in Vector pos)
        {
            var condition = (Math.Floor(pos.Z) + Math.Floor(pos.X)) % 2 != 0;
            var color = condition ? Color.White : Color.Black;
            var reflect = condition ? 0.1 : 0.7;

            return new SurfaceProperties()
            {
                Diffuse = color,
                Specular = Color.White,
                Reflect = reflect,
                Roughness = 250
            };
        }
    }

}