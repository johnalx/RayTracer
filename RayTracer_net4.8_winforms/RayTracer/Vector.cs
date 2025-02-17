using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;

namespace JA.RayTracer
{
    internal struct Vector
    {
        public double X;
        public double Y;
        public double Z;

        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double Dot(Vector v) => X * v.X + Y * v.Y + Z * v.Z;

        public double Length() => Math.Sqrt(X * X + Y * Y + Z * Z);

        public static Vector operator -(Vector a, Vector b) => new Vector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Vector operator +(Vector a, Vector b) => new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Vector operator *(double k, Vector v) => new Vector(k * v.X, k * v.Y, k * v.Z);

        public Vector Norm()
        {            
            var length = Length();
            var div = length == 0 ? double.PositiveInfinity : 1.0 / length;
            return div * this;
        }

        public Vector Cross(Vector v)
        {
            return new Vector(
                Y * v.Z - Z * v.Y,
                Z * v.X - X * v.Z,
                X * v.Y - Y * v.X
            );
        }
    }
}