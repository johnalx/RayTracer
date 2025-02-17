namespace JA.RayTracer.Geometry
{
    internal class Ray
    {
        public Vector Start;
        public Vector Dir;

        public Ray(in Vector start, in Vector dir)
        {
            Start = start;
            Dir = dir;
        }
    }
}