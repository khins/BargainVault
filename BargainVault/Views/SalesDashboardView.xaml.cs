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
    /// Interaction logic for SalesDashboardView.xaml
    /// </summary>
    public partial class SalesDashboardView : Window
    {
        public SalesDashboardView(SalesDashboardViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void DashboardGrid_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Ensure DataContext is correct
            if (DataContext is not SalesDashboardViewModel)
                return;

            // Ensure sender is DataGrid
            if (sender is not DataGrid grid)
                return;

            // Ensure a row is selected
            if (grid.SelectedItem is not SalesMonthlySummaryDto selected)
                return;

            // Resolve required services
            var salesService = App.Services.GetRequiredService<ISalesService>();

            // Create ViewModel for drill-down
            var detailViewModel = new SalesMonthDetailViewModel(
                salesService,
                selected.Year,
                selected.Month);

            // Create and show view
            var detailView = new SalesMonthDetailView(detailViewModel)
            {
                Owner = this
            };

            detailView.ShowDialog();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
