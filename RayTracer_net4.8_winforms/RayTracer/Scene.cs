namespace JA.RayTracer
{
    using Light = JA.RayTracer.Graphics.Light;
    using Color = JA.RayTracer.Graphics.Color;
    using Sphere = JA.RayTracer.Geometry.Sphere;
    using Plane = JA.RayTracer.Geometry.Plane;

    internal class Scene
    {
        public Camera Camera { get; set; }
        public readonly Light[] Lights;
        public readonly IThing[] Things;

        public static ISurface Shiny = new ShinySurface();
        public static ISurface Checkerboard = new CheckerboardSurface();

        public Scene()
        {
            Vector pos = new Vector(3.0, 2.0, 4.0);
            Vector lookAt = new Vector(-1.0, 0.5, 0.0);
            Camera = new Camera(pos, lookAt);

            Things = new IThing[] {
                new Plane(new Vector(0.0, 1.0, 0.0), 0.0, Checkerboard),
                new Sphere(new Vector(0.0, 1.0, -0.25), 1.0, Shiny),
                new Sphere(new Vector(-1.0, 0.5, 1.5), 0.5, Shiny)
            };

            Lights = new Light[] {
                new Light(new Vector(-2.0, 2.5, 0.0), new Color(0.49, 0.07, 0.07)),
                new Light(new Vector(1.5, 2.5, 1.5), new Color(0.07, 0.07, 0.49)),
                new Light(new Vector(1.5, 2.5, -1.5), new Color(0.07, 0.49, 0.071)),
                new Light(new Vector(0.0, 3.5, 0.0), new Color(0.21, 0.21, 0.35))
            };
        }
    }
}