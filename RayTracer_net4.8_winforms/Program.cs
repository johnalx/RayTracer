using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JA
{
    using System.Diagnostics;
    using JA.RayTracer;
    using JA.UI;

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Starting RayTracer");
            var sw = Stopwatch.StartNew();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RunningForm1());
            var dt = sw.Elapsed.TotalSeconds;
            Console.WriteLine($"Finished. Elapsed Time {dt:F3} sec, or {RunningForm1.MAX_FRAMES/dt:F2} fps average.");
            //Console.ReadLine();
        }
    }
}
