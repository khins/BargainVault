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
            services.AddTransient<ItemsEntryView>();
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


            // Views / ViewModels
            services.AddSingleton<MainWindow>();
        }
    }
}
