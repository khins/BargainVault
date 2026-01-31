using BargainVault.Domain.Services;
using BargainVault.ViewModels.Acquisitions;
using BargainVault.Views.Acquisitions;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace BargainVault.Views.Acquisitions
{
    public partial class AcquisitionsListView : Window
    {
        public AcquisitionsListView(AcquisitionsListViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            Loaded += async (_, _) => await vm.LoadAsync();
        }

        private async void EditSelected_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not AcquisitionsListViewModel vm ||
                vm.SelectedAcquisition == null)
                return;

            var acquisitionsService =
                App.Services.GetRequiredService<IAcquisitionsService>();

            var itemsService =
                App.Services.GetRequiredService<IItemsService>();

            var dto = await acquisitionsService
                .GetAcquisitionByIdAsync(vm.SelectedAcquisition.AcqId);

            if (dto == null)
                return;

            var entryVm = new AcquisitionsEntryViewModel(
                acquisitionsService,
                itemsService,
                dto);

            var entryView = new AcquisitionsEntryView(entryVm)
            {
                Owner = this
            };

            entryView.ShowDialog();

            await vm.LoadAsync();
        }

    }
}
