using System;
using System.Runtime.InteropServices;

namespace MFS
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        [STAThread]
        static void Main(string[] args)
        {
#if DEBUG
            AllocConsole();
#endif
            using (var game = new Game1())
                game.Run();
        }
    }
#endif
}
