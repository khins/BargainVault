using BargainVault.Domain.Services;
using BargainVault.ViewModels;
using BargainVault.ViewModels.Acquisitions;
using BargainVault.ViewModels.Items;
using BargainVault.ViewModels.Items.BargainVault.ViewModels.Items;
using BargainVault.Views;
using BargainVault.Views.Acquisitions;
using BargainVault.Views.Items;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace BargainVault
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            DispatcherUnhandledException += (sender, args) =>
            {
                MessageBox.Show(
                    args.Exception.ToString(),
                    "Dispatcher Unhandled Exception",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                args.Handled = true;
            };

            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);

            Services = services.BuildServiceProvider();

            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {

            // Domain services
            services.AddSingleton<IItemsService, ItemsService>();
            services.AddTransient<ItemsEntryViewModel>();
            services.AddTransient<Views.ItemsEntryView>();
            services.AddTransient<ItemsListView>();
            services.AddTransient<ItemsListViewModel>();
            services.AddScoped<IAcquisitionsService, AcquisitionsService>();
            // Acquisitions
            services.AddTransient<AcquisitionsEntryViewModel>();
            services.AddTransient<AcquisitionsEntryView>();
            services.AddTransient<AcquisitionsListViewModel>();
            services.AddTransient<AcquisitionsListView>();
            //Sales
            services.AddScoped<ISalesService, SalesService>();
            services.AddTransient<SalesEntryViewModel>();
            services.AddTransient<SalesEntryView>();
            services.AddTransient<SalesDashboardViewModel>();
            services.AddTransient<SalesDashboardView>();

            //Booth service
            services.AddScoped<IBoothsService, BoothsService>();
            //facebook
            services.AddScoped<IAcquisitionsService, AcquisitionsService>();
            services.AddScoped<IFacebookPostsService, FacebookPostsService>();
            services.AddTransient<FacebookPostsEntryViewModel>();
            services.AddTransient<FacebookPostsEntryView>();
            services.AddTransient<FacebookPostsListViewModel>();
            services.AddTransient<FacebookPostsListView>();
            //inventory location
            services.AddScoped<ILookupsService, LookupsService>();
            services.AddScoped<IInventoryLocationsService, InventoryLocationsService>();
            services.AddTransient<InventoryLocationsEntryViewModel>();
            services.AddTransient<InventoryLocationsEntryView>();
            services.AddTransient<InventoryLocationsListViewModel>();
            services.AddTransient<InventoryLocationsListView>();
            services.AddTransient<InventoryLocationsEntryView>();
            //Report services
            services.AddScoped<IReportsService, ReportsService>();
            services.AddTransient<AuctionProfitReportView>();
            services.AddTransient<AuctionProfitReportViewModel>();

            //global search
            services.AddSingleton<IGlobalSearchService, GlobalSearchService>();            
            services.AddTransient<GlobalSearchView>();            
            services.AddTransient<GlobalSearchViewModel>();
            services.AddScoped<ILookupsService, LookupsService>();

            //retail prices
            services.AddScoped<IRetailPricesService, RetailPricesService>();
            services.AddScoped<IStoresService, StoresService>();
            services.AddTransient<RetailPricesEntryView>();
            services.AddTransient<RetailPricesEntryViewModel>();

            // Views / ViewModels
            services.AddSingleton<MainWindow>();
        }
    }
}
