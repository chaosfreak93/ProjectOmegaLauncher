using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Navigation;
using Application = System.Windows.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ProjectOmegaLauncher
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            VersionLabel.Content = "v" + Assembly.GetExecutingAssembly().GetName().Version;

            if (!Directory.Exists(@".\project_omega")) Directory.CreateDirectory(@".\project_omega");

            WebBrowser.LoadCompleted += reloadBrowser;
        }

        void reloadBrowser(object sender, NavigationEventArgs e)
        {
            WebBrowser.Refresh();
        }

        private void LaunchGameButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(@".\project_omega\Game\Project Omega.exe");
                Application.Current.Shutdown();
            }
            catch
            {
                MessageBox.Show("Game is not installed.\nPlease \"Check for Updates\" to download it!",
                    "Game Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LaunchWebsiteButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://BeyondDark.de");
        }

        private void UpdateGameButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@".\Updater.exe", "game");
        }
    }
}