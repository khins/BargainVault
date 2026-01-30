using BargainVault.Domain.Services;
using BargainVault.ViewModels.Items;
using BargainVault.ViewModels.Items.BargainVault.ViewModels.Items;
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

            // Views / ViewModels
            services.AddSingleton<MainWindow>();
        }
    }
}
