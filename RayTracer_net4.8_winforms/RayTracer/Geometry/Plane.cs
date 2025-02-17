namespace JA.RayTracer.Geometry
{
    internal class Plane : IThing
    {
        private readonly Vector m_Normal;
        private readonly double m_Offset;

        public Plane(Vector norm, double offset, ISurface surface)
        {
            m_Normal = norm;
            m_Offset = offset;
            Surface = surface;
        }

        public Intersection Intersect(in Ray ray)
        {
            var denom = m_Normal.Dot(ray.Dir);
            if (denom > 0)
            {
                return null;
            }

            var dist = (m_Normal.Dot(ray.Start) + m_Offset) / -denom;
            return new Intersection(this, ray, dist);
        }

        public Vector Normal(in Vector pos) => m_Normal;

        public ISurface Surface { get; set; }
    }
}