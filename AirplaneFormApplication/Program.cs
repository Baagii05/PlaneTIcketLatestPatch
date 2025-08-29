using System;
using System.Windows.Forms;

namespace AirplaneFormApplication
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Handle unhandled exceptions
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.Run(new MainMenu());
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            // Log the exception but don't show it to user during shutdown
            Console.WriteLine($"Thread exception: {e.Exception.Message}");
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Log the exception but don't show it to user during shutdown
            Console.WriteLine($"Unhandled exception: {e.ExceptionObject}");
        }
    }
}