using System.Windows;

namespace Updater
{
    /// <summary>
    ///     Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Current.ShutdownMode = ShutdownMode.OnLastWindowClose;

            if (e.Args[0] == "game")
                new MainWindow().CheckForGameUpdates();
            else if (e.Args[0] == "launcher")
                new MainWindow().CheckForLauncherUpdates();
            else
                Current.Shutdown();
        }
    }
}