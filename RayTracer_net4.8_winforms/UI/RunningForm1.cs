using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Threading;


namespace JA.UI
{

    using Scene = RayTracer.Scene;
    using RayTracerEngine = RayTracer.RayTracerEngine;
    using Image = RayTracer.Graphics.Image;

    public partial class RunningForm1 : Form
    {
        public const int MAX_FRAMES = 32;
        static readonly Random rng = new Random();
        readonly FpsCounter clock;
        readonly Scene scene;
        readonly RayTracerEngine rayTracer;

        #region Windows API - Running Form
        [StructLayout(LayoutKind.Sequential)]
        public struct WinMessage
        {
            public IntPtr hWnd;
            public Message msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public Point p;
        }

        [System.Security.SuppressUnmanagedCodeSecurity] // We won't use this maliciously
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(out WinMessage msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);
        public void MainLoop()
        {
            // Hook the application's idle event
            Application.Idle += new EventHandler(OnApplicationIdle);
            //System.Windows.Forms.Application.Run(TrackForm);
        }

        private void OnApplicationIdle(object sender, EventArgs e)
        {
            while (AppStillIdle)
            {
                // Render a frame during idle time (no messages are waiting)
                UpdateMachine();
            }
        }

        private bool AppStillIdle
        {
            get
            {
                WinMessage msg;
                return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
            }
        }
        #endregion

        public RunningForm1()
        {
            InitializeComponent();

            //Initialize the machine
            this.clock=new FpsCounter();
            this.scene = new Scene();
            this.rayTracer = new RayTracerEngine(scene);
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var size = pictureBox1.ClientSize;

            //var image = new Image(500, 500);
            var image = new Image(size.Width, size.Height);
            await rayTracer.Render(image);
            pictureBox1.Image = image.Export();

            MainLoop();
        }

        void UpdateMachine()
        {
            if (clock.Frames == MAX_FRAMES)
            {
                this.Close();
            }
#if ASYNC
            CancellationTokenSource cts = new CancellationTokenSource();
            Task.Run(async () =>
            {
                var size = pictureBox1.ClientSize;
                scene.Camera.Pos += 0.01 * scene.Camera.Forward;
                Image image = await RenderImage(size);
                pictureBox1.Image = image.Export();
            }, cts.Token);
#else
            var size = pictureBox1.ClientSize;
            scene.Camera.Pos += 0.01 * scene.Camera.Forward;
            Image image = Task<Image>.Run( () => RenderImage(size) ).Result;
            pictureBox1.Image = image.Export();
#endif            
        }

        private async Task<Image> RenderImage(Size size)
        {
            var image = new Image(size.Width, size.Height);
            await rayTracer.Render(image);
            return image;
        }

        private void pic_SizeChanged(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void pic_Paint(object sender, PaintEventArgs e)
        {
            // Show FPS counter
            var fps = clock.Measure();
            var text = $"frame:{clock.Frames}   fps:{fps:F2}";
            var sz = e.Graphics.MeasureString(text, SystemFonts.DialogFont);
            var pt = new PointF(pictureBox1.Width-1 - sz.Width - 4, 4);
            e.Graphics.DrawString(text, SystemFonts.DialogFont, Brushes.Yellow, pt);

            // Draw the machine
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;            

        }

        private void pic_MouseClick(object sender, MouseEventArgs e)
        {
        }
    }

}
