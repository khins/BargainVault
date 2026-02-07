using BargainVault.Domain.Models;
using BargainVault.Domain.Models.BargainVault.Domain.Models;
using BargainVault.UI;
using BargainVault.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BargainVault.Views
{
    /// <summary>
    /// Interaction logic for GlobalSearchView.xaml
    /// </summary>
    public partial class GlobalSearchView : Window
    {
        public GlobalSearchView(GlobalSearchViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            Loaded += (_, _) => SearchTextBox.Focus();
        }

        private void Results_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is not GlobalSearchViewModel vm ||
                ((System.Windows.Controls.ListBox)sender).SelectedItem
                    is not GlobalSearchResultDto result)
                return;

            // Route based on entity type
            switch (result.EntityType)
            {
                case GlobalSearchEntityType.Item:
                    NavigationHelper.OpenItem(result.EntityId);
                    break;

                case GlobalSearchEntityType.Acquisition:
                    NavigationHelper.OpenAcquisition(result.EntityId);
                    break;

                //case GlobalSearchEntityType.Sale:
                //    NavigationHelper.OpenSale(result.EntityId);
                //    break;

                case GlobalSearchEntityType.FacebookPost:
                    NavigationHelper.OpenFacebookPost(result.EntityId);
                    break;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
