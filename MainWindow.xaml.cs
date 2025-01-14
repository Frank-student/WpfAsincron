using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfAsincron
{
    public partial class MainWindow : Window
    {
        private bool isPaused = false;
        private CancellationTokenSource? cts;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            isPaused = false;
            cts = new CancellationTokenSource();
            var token = cts.Token;
            var progress = new Progress<int>(value => progres.Value = value);

            try
            {
                string text = text_originall.Text;
                string result = await Cripting.CriptareAsync(text, token, progress);
                var parts = result.Split('|');
                text_crypted.Text = parts[0];
                text_decrypted.Tag = parts[1]; // Store the key in the Tag property
                lbl_text_afis.Content = "Encryption Finalized!";
            }
            catch (OperationCanceledException)
            {
                lbl_text_afis.Content = "Encryption Canceled!";
                text_crypted.Text += " (Invalid)";
            }
        }

        private async void btn_Decrypt_Click(object sender, RoutedEventArgs e)
        {
            isPaused = false;
            cts = new CancellationTokenSource();
            var token = cts.Token;
            var progress = new Progress<int>(value => progres.Value = value);

            try
            {
                string text = text_crypted.Text;
                string key = (string)text_decrypted.Tag;
                string result = await Cripting.DecriptareAsync(text, key, token, progress);
                text_decrypted.Text = result;
                lbl_text_afis.Content = "Decryption Finalized!";
            }
            catch (OperationCanceledException)
            {
                lbl_text_afis.Content = "Decryption Canceled!";
                text_decrypted.Text += " (Invalid)";
            }
        }

        private void btn_Freeze_Click(object sender, RoutedEventArgs e)
        {
            isPaused = true;
        }

        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            cts?.Cancel();
            isPaused = false;
        }

        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            text_crypted.Text = string.Empty;
            text_decrypted.Text = string.Empty;
            lbl_text_afis.Content = "Cleared!";
            progres.Value = 0;
        }

        private async void btn_LoadFile_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "path_to_your_file.txt"; // Update with your file path
            string content = await FileHelper.ReadFileAsync(filePath);
            text_originall.Text = content;
        }

        private async void btn_SaveFile_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "path_to_your_file.txt"; // Update with your file path
            string content = text_crypted.Text;
            await FileHelper.WriteFileAsync(filePath, content);
        }
    }
}
