using System;

namespace MFS
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string hostname = "127.0.0.1";
            if (args.Length == 1)
            {
                hostname = args[0];
            }

            using (var game = new Game1(hostname))
                game.Run();
        }
    }
#endif
}
