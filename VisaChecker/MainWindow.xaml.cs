﻿using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace VisaChecker
{

    public partial class MainWindow : Window
    {
        private DispatcherTimer _clipboardMonitorTime;
        private const string ClipBoardBusy = "Clipboard is busy, please try again.";
        private readonly AppDbContext _context;
        private CollectionViewSource categoryViewSource;
        private const int MaxClipboardTextLength = 15;

        public MainWindow(AppDbContext context)
        {
            InitializeComponent();
            _context = context;
            categoryViewSource =
                (CollectionViewSource)FindResource(nameof(categoryViewSource));
            inputTextBox.Focus();
            _clipboardMonitorTime = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Title += $" {version}";
        }

        private void LoadData()
        {
            try
            {
                _context.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;
                var data = _context.Gov.Where(c => c.Name.Contains(inputTextBox.Text)).OrderBy(x => x.Name).ToList();

                categoryDataGrid.ItemsSource = data;
                itemCount.Text = data.Count().ToString();
                Activate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void ClipboardMonitorTime_Tick(object? sender, EventArgs e)
        {
            try
            {
                var clipboardText = Clipboard.GetText();

                if (inputTextBox.Text == clipboardText)
                    return;

                if (clipboardText.Length > MaxClipboardTextLength)
                    clipboardText = clipboardText.Substring(0, MaxClipboardTextLength-1);

                inputTextBox.Text = clipboardText;
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                itemCount.Text = ClipBoardBusy;
            }
            catch (Exception ex)
            {
                itemCount.Text = ex.Message;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _clipboardMonitorTime.Tick += ClipboardMonitorTime_Tick;
            _clipboardMonitorTime.Start();
            status.Text = "Monitoring...";

        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _clipboardMonitorTime.Tick -= ClipboardMonitorTime_Tick;
            _clipboardMonitorTime.Stop();
            status.Text = "";
        }

        private void inputTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (inputTextBox.Text.Length == 0)
            {
                categoryDataGrid.ItemsSource = null;
                itemCount.Text = "0";
            }
            if (inputTextBox.Text.Length < 4)
                return;

            LoadData();
        }
    }
}