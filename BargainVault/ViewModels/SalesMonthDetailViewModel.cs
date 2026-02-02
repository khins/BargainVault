using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BargainVault.ViewModels
{
    public class SalesMonthDetailViewModel : ViewModelBase
    {
        private readonly ISalesService _salesService;

        public ObservableCollection<SalesMonthlyDetailDto> Sales { get; }
            = new();

        public int Year { get; }
        public int Month { get; }

        public SalesMonthDetailViewModel(
            ISalesService salesService,
            int year,
            int month)
        {
            _salesService = salesService;
            Year = year;
            Month = month;

            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            Sales.Clear();

            var results = await _salesService.GetSalesForMonthAsync(Year, Month);
            foreach (var row in results)
                Sales.Add(row);

            OnPropertyChanged(nameof(TotalUnitPrice));
            OnPropertyChanged(nameof(TotalLineTotal));
        }

        public decimal TotalUnitPrice
                 => Sales.Sum(s => s.UnitSalePrice);

        public decimal TotalLineTotal
                  => Sales.Sum(s => s.LineTotal);

    }

}
