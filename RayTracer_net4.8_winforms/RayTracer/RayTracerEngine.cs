//#define PARALLEL

using System;
using System.Threading.Tasks;
using JA.RayTracer.Geometry;

namespace JA.RayTracer
{
    using Color = JA.RayTracer.Graphics.Color;
    using Image = JA.RayTracer.Graphics.Image;
    using Light = JA.RayTracer.Graphics.Light;

    internal class RayTracerEngine
    {
        private const int maxDepth = 5;
        private Scene scene;

        public RayTracerEngine(Scene scene)
        {
            this.scene = scene;
        }

        private Intersection Intersections(Ray ray)
        {
            var closest = double.PositiveInfinity;
            Intersection closestInter = null;

            for (int i = 0; i < scene.Things.Length; ++i)
            {
                var inter = scene.Things[i].Intersect(ray);
                if (inter != null && inter.Dist < closest)
                {
                    closestInter = inter;
                    closest = inter.Dist;
                }
            }

            return closestInter;
        }

        private Color TraceRay(Ray ray, int depth)
        {
            var isect = Intersections(ray);
            return isect != null ? Shade(isect, depth) : Color.Background;
        }

        private Color Shade(Intersection isect, int depth)
        {
            Vector d = isect.Ray.Dir;

            var pos = isect.Dist * d + isect.Ray.Start;
            var normal = isect.Thing.Normal(pos);
            var reflectDir = d - 2 * normal.Dot(d) * normal;

            var surface = isect.Thing.Surface.GetSurfaceProperties(pos);
            var naturalColor = Color.Background + GetNaturalColor();

            var reflectedColor = depth >= maxDepth ? Color.Grey : GetReflectionColor();
            return naturalColor + reflectedColor;

            Color GetReflectionColor()
            {
                Ray ray = new Ray(pos, reflectDir);
                return surface.Reflect * TraceRay(ray, depth + 1);
            }

            Color GetNaturalColor()
            {
                var result = Color.Defaultcolor;
                for (int i = 0; i < scene.Lights.Length; ++i)
                {
                    result = AddLight(result, scene.Lights[i]);
                }
                return result;
            }

            Color AddLight(Color col, Light light)
            {
                var ldis = light.Pos - pos;
                var livec = ldis.Norm();
                Ray ray = new Ray(pos, livec);
                var neatIsect = Intersections(ray);

                var isInShadow = neatIsect != null && neatIsect.Dist <= ldis.Length();
                if (isInShadow)
                {
                    return col;
                }
                var illum = livec.Dot(normal);
                var lcolor = illum > 0 ? illum * light.Color : Color.Defaultcolor;

                var specular = livec.Dot(reflectDir.Norm());
                var scolor = specular > 0 ? Math.Pow(specular, surface.Roughness) * light.Color : Color.Defaultcolor;

                return col + surface.Diffuse * lcolor + surface.Specular * scolor;
            }
        }

        public async Task Render(Image image)
        {
            int w = image.Width;
            int h = image.Height;
            Ray ray = new Ray(scene.Camera.Pos, new Vector(0, 0, 0));

#if PARALLEL
            Parallel.For(0, h, (y) =>
            {
                int pos = y * w;
                for (var x = 0; x < w; ++x)
                {
                    ray.Dir = scene.Camera.GetPoint(x, y, w, h);
                    var color = TraceRay(ray, 0);
                    image[pos + x] = color.ToRGBColor();
                }
            });            
#else
            await Task.Run(() =>
            {
                for (var y = 0; y < h; ++y)
                {
                    int pos = y * w;
                    for (var x = 0; x < w; ++x)
                    {
                        ray.Dir = scene.Camera.GetPoint(x, y, w, h);
                        var color = TraceRay(ray, 0);
                        image[pos + x] = color.ToRGBColor();
                    }
                }
            });
#endif
        }
    }
}