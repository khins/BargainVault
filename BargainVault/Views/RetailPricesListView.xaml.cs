using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BargainVault.Views
{
    /// <summary>
    /// Interaction logic for RetailPricesListView.xaml
    /// </summary>
    public partial class RetailPricesListView : Window
    {
        private readonly RetailPricesListViewModel _vm;
        public RetailPricesListView(RetailPricesListViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = vm;

            Loaded += async (_, _) => await _vm.LoadAsync();
        }

        private async void EditSelected_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not RetailPricesListViewModel vm ||
                vm.SelectedItem == null)
                return;

            var service = App.Services.GetRequiredService<IRetailPricesService>();

            var dto = await service.GetRetailPriceByIdAsync(vm.SelectedItem.RetailPriceId);

            if (dto == null)
                return;

            var entryVm = new RetailPricesEntryViewModel(
                service,
                App.Services.GetRequiredService<IItemsService>(),
                App.Services.GetRequiredService<IStoresService>(),
                dto);

            var entryView = new RetailPricesEntryView(entryVm)
            {
                Owner = this
            };

            entryView.ShowDialog();

            await vm.LoadAsync();   // refresh list
        }


        private async void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not RetailPricesListViewModel vm ||
                vm.SelectedItem == null)
                return;

            var confirm = MessageBox.Show(
                "Delete this retail price?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes)
                return;

            await vm.DeleteSelectedAsync(Environment.UserName);

            await vm.LoadAsync();
        }

    }
}
