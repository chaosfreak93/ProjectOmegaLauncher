using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ProjectOmegaLauncher
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly bool installing;
        private bool newer;

        public MainWindow()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            VersionLabel.Content = "v" + Assembly.GetExecutingAssembly().GetName().Version;

            if (!Directory.Exists(@".\project_omega"))
            {
                Directory.CreateDirectory(@".\project_omega");
                installing = true;
                var install = new Install();
                install.Show();
            }

            if (!installing) UpdateCheck();
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
            UpdateCheck();
        }

        private void UpdateCheck()
        {
            using (var wc = new WebClient())
            {
                wc.DownloadFileAsync(
                    // Param1 = Link of file
                    new Uri("http://5.181.151.36/project_omega/Game/ServerVersion.txt"),
                    // Param2 = Path to save
                    @".\project_omega\ServerVersion.txt"
                );

                wc.DownloadFileCompleted += wc_DownloadFileCompleted;
            }
        }

        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                var localTextFile = File.ReadAllText(@".\project_omega\Game\LocalVersion.txt");
                var serverTextFile = File.ReadAllText(@".\project_omega\ServerVersion.txt");

                if (localTextFile != serverTextFile)
                    newer = true;
                else
                    newer = false;
                File.Delete(@".\project_omega\ServerVersion.txt");

                if (newer)
                {
                    var update = new Update();
                    update.Show();
                }
                else
                {
                    MessageBox.Show("You already have the latest Game Version!", "Uptodate", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch
            {
                Console.WriteLine("Some errors on update check!");
            }
        }
    }
}