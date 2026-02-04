using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.Views;
using BargainVault.Views.Acquisitions;
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

        private void OpenItemsList_Click(object sender, RoutedEventArgs e)
        {
            var view = App.Services.GetRequiredService<ItemsListView>();
            view.ShowDialog();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void TestAcquisitionInsert_Click(object sender, RoutedEventArgs e)
        {
            var service = App.Services.GetRequiredService<IAcquisitionsService>();

            var id = await service.InsertAcquisitionAsync(
                itemId: 713,
                sourceType: "Auction",
                auctionSiteId: 1,
                dateAcquired: DateTime.Today,
                qtyAcquired: 1,
                unitHammerPrice: 100m,
                buyerPremium: 10m,
                taxRate: 0.0825m,
                salesTaxPaid: 9.90m,
                totalSettlement: 119.90m,
                statusId: 1,
                personal: false,
                businessExpense: true,
                enteredBy: Environment.UserName
            );

            MessageBox.Show($"Inserted Acquisition ID: {id}");
        }

        private void OpenAcquisitionEntry_Click(object sender, RoutedEventArgs e)
        {
            var view = App.Services.GetRequiredService<AcquisitionsEntryView>();
            view.Owner = this;
            view.ShowDialog();
        }

        private void OpenAcquisitionsList_Click(object sender, RoutedEventArgs e)
        {
            var view = App.Services.GetRequiredService<AcquisitionsListView>();
            view.Owner = this;
            view.ShowDialog();
        }

        private void OpenSalesEntry_Click(object sender, RoutedEventArgs e)
        {
            var view = App.Services.GetRequiredService<SalesEntryView>();
            view.Owner = this;
            view.ShowDialog();
        }

        private void OpenSalesDashboard_Click(object sender, RoutedEventArgs e)
        {
            var view = App.Services.GetRequiredService<SalesDashboardView>();
            view.Owner = this;
            view.ShowDialog();
        }

        // 🧪 Step 4: Quick Service Sanity Test 
        private async void TestFacebookPostInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var service = App.Services.GetRequiredService<IFacebookPostsService>();

                var newId = await service.InsertFacebookPostAsync(
                    new FacebookPostDto
                    {
                        AcqId = 1, // must exist
                        PostDate = DateTime.Now,
                        PostTitle = "Test Facebook Post",
                        PostDescription = "Sanity test insert",
                        AskingPrice = 49.99m,
                        Boosted = false,
                        MarkAsSold = false
                    },
                    Environment.UserName);

                MessageBox.Show($"Inserted Facebook Post ID: {newId}",
                                "Success",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                                "Insert Failed",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void OpenFacebookPostInsert_Click(object sender, RoutedEventArgs e)
        {
            var view = App.Services.GetRequiredService<FacebookPostsEntryView>();
            view.Owner = this;
            view.ShowDialog();
        }

        private void OpenFacebookPostsList_Click(object sender, RoutedEventArgs e)
        {
            var view = App.Services.GetRequiredService<FacebookPostsListView>();
            view.Owner = this;
            view.ShowDialog();
        }

        private void NewInventoryItem_Click(object sender, RoutedEventArgs e)
        {
            var view = App.Services.GetRequiredService<InventoryLocationsEntryView>();
            view.Owner = this;
            view.ShowDialog();
        }
    }
}