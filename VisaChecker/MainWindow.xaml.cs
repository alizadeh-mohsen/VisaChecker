using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace VisaChecker
{

    public partial class MainWindow : Window
    {

        private DispatcherTimer _clipboardMonitorTime;
        private const string ClipBoardBusy = "Clipboard is busy, please try again.";
        private readonly ProductContext _context;
        private CollectionViewSource categoryViewSource;

        public MainWindow(ProductContext context)
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

            _clipboardMonitorTime.Tick += ClipboardMonitorTime_Tick;
            _clipboardMonitorTime.Start();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                if (inputTextBox.Text.Length > 15)
                {
                    MessageBox.Show("Please enter a search term.");
                    return;
                }

                var data = _context.Gov.Where(c => c.Name.Contains(inputTextBox.Text)).OrderBy(x => x.Name).ToList();

                categoryDataGrid.ItemsSource = data;
                itemCount.Text = data.Count().ToString();
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
                if (Clipboard.ContainsText() && Clipboard.GetText().Length < 15)
                {
                    inputTextBox.Text = Clipboard.GetText();
                    Clipboard.Clear(); // Clear clipboard to prevent repeated reads
                    LoadData();
                }
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
    }
}