using System;
using System.ComponentModel;
using System.Diagnostics;
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
    public partial class UpdateLauncher
    {
        public UpdateLauncher()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            using (var wc = new WebClient())
            {
                File.Delete(@".\Launcher.zip");

                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileAsync(
                    // Param1 = Link of file
                    new Uri("http://5.181.151.36/project_omega/Launcher/Launcher.zip"),
                    // Param2 = Path to save
                    @".\Launcher.zip"
                );

                wc.DownloadFileCompleted += wc_DownloadFileCompleted;
            }
        }

        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                var zipPath = @".\Launcher.zip";
                var extractPath = @".\";

                File.Delete(@".\ProjectOmegaLauncher.exe");

                ZipFile.ExtractToDirectory(zipPath, extractPath);

                File.Delete(zipPath);

                Close();

                var mg = MessageBox.Show("Launcher has been updated!", "Done", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                if (mg == System.Windows.Forms.DialogResult.OK)
                {
                    Process.Start(@".\ProjectOmegaLauncher.exe", "no-update");
                    Application.Current.Shutdown();
                }
            }
            catch
            {
                var mg = MessageBox.Show(
                    "Installation errors occurred.", "Installing failed",
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