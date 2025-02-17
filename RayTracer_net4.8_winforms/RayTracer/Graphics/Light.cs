namespace JA.RayTracer.Graphics
{
    internal struct Light
    {
        public Vector Pos;
        public Color Color;

        public Light(Vector pos, Color color)
        {
            Pos = pos;
            Color = color;
        }
    }
}