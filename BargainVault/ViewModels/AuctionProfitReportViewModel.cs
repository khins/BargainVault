using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;

namespace BargainVault.ViewModels
{
    public class AuctionProfitReportViewModel : ViewModelBase
    {
        private readonly IReportsService _reportsService;

        public ObservableCollection<AuctionProfitDto> Results { get; }
            = new();

        public AuctionProfitReportViewModel(IReportsService reportsService)
        {
            _reportsService = reportsService;
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            Results.Clear();
            var results = await _reportsService.GetAuctionProfitAnalysisAsync();
            foreach (var row in results)
                Results.Add(row);

            Items = new ObservableCollection<AuctionProfitDto>(results);

            ItemsView = CollectionViewSource.GetDefaultView(Items);
            ItemsView.Filter = FilterItems;

            OnPropertyChanged(nameof(ItemsView));
        }

        private ObservableCollection<AuctionProfitDto> _items;
        public ObservableCollection<AuctionProfitDto> Items
        {
            get => _items;
            private set
            {
                _items = value;
                OnPropertyChanged();
            }
        }

        public ICollectionView ItemsView { get; private set; }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ItemsView?.Refresh();
            }
        }

        private bool FilterItems(object obj)
        {
            if (obj is not AuctionProfitDto item)
                return false;

            if (string.IsNullOrWhiteSpace(SearchText))
                return true;

            var text = SearchText.Trim().ToLower();

            return
                item.Title?.ToLower().Contains(text) == true ||
                item.AuctionSite?.ToLower().Contains(text) == true ||
                item.LotNumber?.ToString().Contains(text) == true;
        }


    }

}
