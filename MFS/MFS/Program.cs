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
        //TODO: How to show it in uml?
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
            string hostname = "127.0.0.1";
            if (args.Length == 1)
            {
                hostname = args[0];
            }

            //TODO: what does run() does specifically
            using (var game = new Game1(hostname))
                game.Run();
        }
    }
#endif
}
