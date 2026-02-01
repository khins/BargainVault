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
    /// Interaction logic for SalesEntryView.xaml
    /// </summary>
    public partial class SalesEntryView : Window
    {
        public SalesEntryView(SalesEntryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
