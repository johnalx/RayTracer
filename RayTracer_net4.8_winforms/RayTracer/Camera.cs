namespace JA.RayTracer
{
    internal class Camera
    {
        public Vector Forward;
        public Vector Right;
        public Vector Up;
        public Vector Pos;

        public Camera(Vector pos, Vector lookAt)
        {
            var up = new Vector(0.0, 1.0, 0.0);
            Pos = pos;
            Forward = (lookAt - Pos).Norm();
            Right = 1.5 * up.Cross(Forward).Norm();
            Up = 1.5 * Forward.Cross(Right).Norm();
        }

        public Vector GetPoint(int x, int y, int w, int h)
        {
            var recenterX = (x - w / 2.0) / 2.0 / w;
            var recenterY = -(y - h / 2.0) / 2.0 / h;
            return (Forward + recenterX * Right + recenterY * Up).Norm();
        }
    }
}