using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VisaChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ProductContext _context;
        private CollectionViewSource categoryViewSource;
        public MainWindow(ProductContext context)
        {

            InitializeComponent();
            _context = context;
            categoryViewSource =
                (CollectionViewSource)FindResource(nameof(categoryViewSource));
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                if (string.IsNullOrWhiteSpace(inputTextBox.Text) || inputTextBox.Text.Length > 15)
                {
                    MessageBox.Show("Please enter a search term.");
                    return;
                }
                // Clear the DataGrid at the start of Button_Click

                _context.Gov.Local.Clear(); 

                _context.Gov.Where(c => c.Name.Contains(inputTextBox.Text)).Load();
                categoryViewSource.Source = _context.Gov.Local.ToObservableCollection().Where(c => c.Name.Contains(inputTextBox.Text));

                categoryDataGrid.Items.Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    } }