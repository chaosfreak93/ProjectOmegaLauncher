
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
            MainWindow main = new MainWindow(e.Args[0]);
            main.Show();
        }
    }
}