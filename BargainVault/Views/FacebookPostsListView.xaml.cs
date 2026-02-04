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
    public partial class FacebookPostsListView : Window
    {
        public FacebookPostsListView(FacebookPostsListViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private async void EditSelected_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not FacebookPostsListViewModel vm ||
                vm.SelectedPost == null)
                return;

            var service = App.Services.GetRequiredService<IFacebookPostsService>();
            var dto = await service.GetFacebookPostByIdAsync(vm.SelectedPost.PostId);
            if (dto == null)
                return;

            var entryView = new FacebookPostsEntryView(
                new FacebookPostsEntryViewModel(
                    service,
                    App.Services.GetRequiredService<IAcquisitionsService>(),
                    dto))
            {
                Owner = this
            };

            entryView.ShowDialog();
            await vm.LoadAsync();
        }

        private async void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not FacebookPostsListViewModel vm ||
                vm.SelectedPost == null)
                return;

            if (MessageBox.Show("Delete this Facebook post?",
                                "Confirm Delete",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            await vm.DeleteSelectedAsync();
        }
    }

}
