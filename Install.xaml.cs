﻿using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ProjectRebootLauncher
{
    /// <summary>
    ///     Interaktionslogik für Download.xaml
    /// </summary>
    public partial class Install : Window
    {
        public Install()
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
                    @".\Project_Omega\Game.zip"
                );

                wc.DownloadFileCompleted += wc_DownloadFileCompleted;
            }
        }

        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                var zipPath = @".\Project_Omega\Game.zip";
                var extractPath = @".\Project_Omega\Game";

                ZipFile.ExtractToDirectory(zipPath, extractPath);

                File.Delete(zipPath);

                Close();

                MessageBox.Show("Game is now installed!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show(
                    "Installation errors occurred.\nPlease move to \"" + Directory.GetCurrentDirectory() +
                    "\" and remove the \"Project_Omega\" Folder.\nThen try again!!!", "Installing failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            lblProgress.Content = string.Format("Downloaded {0} of {1}",
                Utilities.FormatBytes(e.BytesReceived, 1, true), Utilities.FormatBytes(e.TotalBytesToReceive, 1, true));
            progressBar.Value = e.ProgressPercentage;
        }
    }
}