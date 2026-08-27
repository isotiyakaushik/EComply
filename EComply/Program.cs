using EComply.Common;

namespace EComply
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // UI thread ના exceptions માટે (WinForms controls, event handlers)
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;

            // Non-UI thread (background thread) ના exceptions માટે
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Async/Task ના unobserved exceptions માટે
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            if (!Directory.Exists(Path.Combine(Statics.AppPath, "Database")))
            {
                Directory.CreateDirectory(Path.Combine(Statics.AppPath, "Database"));
            }

            ManageMasterDB Main = new ManageMasterDB();
            Main.Table();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MDIParent1());
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Error.HandleShow(e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Error.HandleShow(e.ExceptionObject as Exception);
        }

        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Error.HandleShow(e.Exception);
            e.SetObserved(); // process crash થતું અટકાવે છે
        }
    }
}