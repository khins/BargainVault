using BargainVault.Domain.Services;
using BargainVault.ViewModels;
using BargainVault.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace BargainVault.Views
{
    public partial class InventoryLocationsListView : Window
    {
        public InventoryLocationsListView(InventoryLocationsListViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private async void EditSelected_Click(object sender, MouseButtonEventArgs e)
        {
            await OpenEditViewAsync();
        }

        private async void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not InventoryLocationsListViewModel vm ||
                vm.SelectedLocation == null)
                return;

            if (MessageBox.Show(
                    "Delete this inventory location?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            await vm.DeleteSelectedAsync();
        }

        private async void EditSelected_Click(object sender, RoutedEventArgs e)
        {
            await OpenEditViewAsync();
        }

        private async Task OpenEditViewAsync()
        {
            if (DataContext is not InventoryLocationsListViewModel vm ||
                vm.SelectedLocation == null)
                return;

            var service = App.Services.GetRequiredService<IInventoryLocationsService>();
            var dto = await service.GetInventoryLocationByIdAsync(
                vm.SelectedLocation.InventoryLocationId);

            if (dto == null)
                return;

            var entryView = App.Services.GetRequiredService<InventoryLocationsEntryView>();
            entryView.DataContext = new InventoryLocationsEntryViewModel(
                service,
                App.Services.GetRequiredService<IItemsService>(),
                App.Services.GetRequiredService<ILookupsService>(),
                dto);

            entryView.Owner = this;
            entryView.ShowDialog();

            await vm.LoadAsync();
        }
    }

}
