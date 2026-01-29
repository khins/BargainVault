using BargainVault.Domain.Services;
using BargainVault.Views.Items;
using Microsoft.Extensions.DependencyInjection;
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

namespace BargainVault
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += async (_, _) =>
            {
                var itemsService = App.Services.GetRequiredService<IItemsService>();


                //var success = await itemsService.TestConnectionAsync();

                //MessageBox.Show(
                //    success ? "Database connection OK ✅" : "Database connection FAILED ❌",
                //    "BargainVault – DB Test",
                //    MessageBoxButton.OK,
                //    success ? MessageBoxImage.Information : MessageBoxImage.Error
                //);
            };
        }

        private void OpenItemsEntry_Click(object sender, RoutedEventArgs e)
        {
            var view = App.Services.GetRequiredService<ItemsEntryView>();
            view.ShowDialog();
        }
    }
}