using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Updater
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private bool _newer;

        public MainWindow(string toUpdate)
        {
            if (toUpdate == "game")
            {
                CheckForGameUpdates();
            }
            else
            {
                CheckForLauncherUpdates();
            }
        }
        
        public void CheckForLauncherUpdates()
        {
            using (var wc = new WebClient())
            {
                wc.DownloadFileAsync(
                    // Param1 = Link of file
                    new Uri("http://5.181.151.36/project_omega/Launcher/ServerVersion.txt"),
                    // Param2 = Path to save
                    @".\ServerVersion.txt", "launcher"
                );

                wc.DownloadFileCompleted += wc_DownloadFileCompleted;
            }
        }
        
        public void CheckForGameUpdates()
        {
            using (var wc = new WebClient())
            {
                wc.DownloadFileAsync(
                    // Param1 = Link of file
                    new Uri("http://5.181.151.36/project_omega/Game/ServerVersion.txt"),
                    // Param2 = Path to save
                    @".\project_omega\ServerVersion.txt", "game"
                );

                wc.DownloadFileCompleted += wc_DownloadFileCompleted;
            }
        }

        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.UserState.ToString() == "game")
            {
                try
                {
                    if (!Directory.Exists(@".\project_omega\Game"))
                    {
                        File.Delete(@".\project_omega\ServerVersion.txt");
                        var installGame = new InstallGame();
                        installGame.Show();
                        return;
                    }
                    
                    var localGameVersion = File.ReadAllText(@".\project_omega\Game\LocalVersion.txt");
                    var serverGameVersion = File.ReadAllText(@".\project_omega\ServerVersion.txt");

                    if (localGameVersion != serverGameVersion)
                        _newer = true;
                    else
                        _newer = false;
                    File.Delete(@".\project_omega\ServerVersion.txt");

                    if (_newer)
                    {
                        var updateGame = new UpdateGame();
                        updateGame.Show();
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
            } else if (e.UserState.ToString() == "launcher")
            {
                try
                {
                    var localGameVersion = FileVersionInfo.GetVersionInfo(@".\ProjectOmegaLauncher.exe").ProductVersion;
                    var serverGameVersion = File.ReadAllText(@".\ServerVersion.txt");

                    if (localGameVersion != serverGameVersion)
                        _newer = true;
                    else
                        _newer = false;
                    File.Delete(@".\ServerVersion.txt");

                    if (_newer)
                    {
                        var updateLauncher = new UpdateLauncher();
                        updateLauncher.Show();
                    }
                    else
                    {
                        MessageBox.Show("You already have the latest Launcher Version!", "Uptodate", MessageBoxButtons.OK,
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
}