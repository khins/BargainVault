using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BargainVault.ViewModels
{
    public class SalesDashboardViewModel : ViewModelBase
    {
        private readonly ISalesService _salesService;

        public ObservableCollection<SalesMonthlySummaryDto> MonthlySales { get; }
            = new();

        private decimal _totalSales;
        public decimal TotalSales
        {
            get => _totalSales;
            set => SetProperty(ref _totalSales, value);
        }

        public SalesDashboardViewModel(ISalesService salesService)
        {
            _salesService = salesService;
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            MonthlySales.Clear();

            var results = await _salesService.GetMonthlySalesSummaryAsync();
            foreach (var row in results)
                MonthlySales.Add(row);

            TotalSales = MonthlySales.Sum(x => x.TotalSales);
        }
    }

}
