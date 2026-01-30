using BargainVault.Domain.Services;
using BargainVault.ViewModels.Items;
using BargainVault.ViewModels.Items.BargainVault.ViewModels.Items;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace BargainVault.Views.Items
{
    public partial class ItemsListView : Window
    {
        public ItemsListView(ItemsListViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            Loaded += async (_, _) =>
            {
                await viewModel.LoadAsync();
            };
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is not ItemsListViewModel vm || vm.SelectedItem == null)
                return;

            var editView = App.Services.GetRequiredService<ItemsEntryView>();

            // Inject selected item into edit mode
            editView.DataContext = new ItemsEntryViewModel(
                App.Services.GetRequiredService<IItemsService>(),
                vm.SelectedItem);

            editView.Owner = this;
            editView.ShowDialog();

            // Refresh list after edit
            _ = vm.LoadAsync();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EditSelected_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not ItemsListViewModel vm || vm.SelectedItem == null)
                return;

            var editView = App.Services.GetRequiredService<ItemsEntryView>();

            editView.DataContext = new ItemsEntryViewModel(
                App.Services.GetRequiredService<IItemsService>(),
                vm.SelectedItem);

            editView.Owner = this;
            editView.ShowDialog();

            // Refresh list after edit
            _ = vm.LoadAsync();
        }
    }
}
