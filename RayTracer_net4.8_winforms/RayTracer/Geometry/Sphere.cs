using System;

namespace JA.RayTracer.Geometry
{
    internal class Sphere : IThing
    {
        private readonly double m_Radius2;
        private readonly Vector m_Center;

        public Sphere(Vector center, double radius, ISurface surface)
        {
            m_Radius2 = radius * radius;
            Surface = surface;
            m_Center = center;
        }

        public Intersection Intersect(in Ray ray)
        {
            var eo = m_Center - ray.Start;
            var v = eo.Dot(ray.Dir);
            var dist = 0.0;

            if (v >= 0)
            {
                var disc = m_Radius2 - (eo.Dot(eo) - v * v);
                if (disc >= 0)
                {
                    dist = v - Math.Sqrt(disc);
                }
            }

            return dist == 0.0 ? null : new Intersection(this, ray, dist);
        }

        public Vector Normal(in Vector pos) => (pos - m_Center).Norm();

        public ISurface Surface { get; set; }
    }
}