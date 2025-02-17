namespace JA.RayTracer.Graphics
{
    /// <summary>
    /// 32-bit color pixel
    /// </summary>
    internal struct RGBColor
    {
        public byte B;
        public byte G;
        public byte R;
        public byte A;
    }

    internal struct Color
    {
        public double R;
        public double G;
        public double B;

        public static Color White = new Color(1.0, 1.0, 1.0);
        public static Color Grey = new Color(0.5, 0.5, 0.5);
        public static Color Black = new Color(0.0, 0.0, 0.0);
        public static Color Background = Color.Black;
        public static Color Defaultcolor = Color.Black;

        public Color(double r, double g, double b)
        {
            R = r;
            G = g;
            B = b;
        }

        public static Color operator *(double k, Color v) => new Color(k * v.R, k * v.G, k * v.B);

        public static Color operator +(Color a, Color b) => new Color(a.R + b.R, a.G + b.G, a.B + b.B);

        public static Color operator *(Color a, Color b) => new Color(a.R * b.R, a.G * b.G, a.B * b.B);

        public RGBColor ToRGBColor()
        {
            return new RGBColor
            {
                B = Clamp(this.B),
                G = Clamp(this.G),
                R = Clamp(this.R),
                A = 255
            };
        }

        public static byte Clamp(double c)
        {
            if (c > 1.0) return 255;
            if (c < 0.0) return 0;
            return (byte)(c * 255);
        }
    }

}