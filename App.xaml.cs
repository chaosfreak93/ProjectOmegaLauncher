using System.Diagnostics;
using System.Windows;

namespace ProjectOmegaLauncher
{
    /// <summary>
    ///     Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length != 1)
            {
                Process.Start(@".\Updater.exe", "launcher");
                Current.Shutdown();
            }
            else
            {
                if (e.Args[0] == "no-update") new MainWindow().Show();
            }
        }
    }
}