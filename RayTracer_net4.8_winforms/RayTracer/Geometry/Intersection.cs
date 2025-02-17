namespace JA.RayTracer.Geometry
{
    internal class Intersection
    {
        public IThing Thing;
        public Ray Ray;
        public double Dist;

        public Intersection(IThing thing, in Ray ray, double dist)
        {
            Thing = thing;
            Ray = ray;
            Dist = dist;
        }
    }
}