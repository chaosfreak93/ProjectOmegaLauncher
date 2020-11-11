using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Updater
{
    /// <summary>
    ///     Interaktionslogik für Download.xaml
    /// </summary>
    public partial class InstallGame
    {
        public InstallGame()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            using (var wc = new WebClient())
            {
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileAsync(
                    // Param1 = Link of file
                    new Uri("http://5.181.151.36/project_omega/Game/Game.zip"),
                    // Param2 = Path to save
                    @".\project_omega\Game.zip"
                );

                wc.DownloadFileCompleted += wc_DownloadFileCompleted;
            }
        }

        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                var zipPath = @".\project_omega\Game.zip";
                var extractPath = @".\project_omega";

                ZipFile.ExtractToDirectory(zipPath, extractPath);

                File.Delete(zipPath);

                Close();

                MessageBox.Show("Game has been installed", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                var mg = MessageBox.Show(
                    "Installation errors occurred.\nPlease move to \"" + Directory.GetCurrentDirectory() +
                    "\" and remove the \"Project_Omega\" Folder.\nThen try again!!!", "Installing failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (mg == System.Windows.Forms.DialogResult.OK) Application.Current.Shutdown();
            }
        }

        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            LblProgress.Content = string.Format("Downloaded {0} of {1}",
                Utilities.FormatBytes(e.BytesReceived, 1, true), Utilities.FormatBytes(e.TotalBytesToReceive, 1, true));
            ProgressBar.Value = e.ProgressPercentage;
        }
    }
}