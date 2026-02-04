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
using System.Windows.Shapes;

namespace BargainVault.Views
{
    /// <summary>
    /// Interaction logic for InventoryLocationsEntryView.xaml
    /// </summary>
    public partial class InventoryLocationsEntryView : Window
    {
        public InventoryLocationsEntryView(InventoryLocationsEntryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
