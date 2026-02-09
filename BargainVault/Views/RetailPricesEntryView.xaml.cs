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
    /// Interaction logic for RetailPricesEntryView.xaml
    /// </summary>
    public partial class RetailPricesEntryView : Window
    {
        public RetailPricesEntryView(RetailPricesEntryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void RetailPrice_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is not TextBox textBox)
                return;

            if (decimal.TryParse(textBox.Text, out var value))
            {
                textBox.Text = value.ToString("C2");
            }
        }

        private void RetailPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !decimal.TryParse(
                ((TextBox)sender).Text + e.Text,
                out _);
        }
    }
}
