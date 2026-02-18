using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.ViewModels.Base;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace BargainVault.ViewModels
{
    public class RetailPricesListViewModel : ViewModelBase
    {
        private readonly IRetailPricesService _service;

        public ObservableCollection<RetailPriceListDto> Items { get; } = new();

        private ICollectionView? _itemsView;
        public ICollectionView? ItemsView
        {
            get => _itemsView;
            private set => SetProperty(ref _itemsView, value);
        }

        private RetailPriceListDto? _selectedItem;
        public RetailPriceListDto? SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnPropertyChanged(nameof(HasSelection));
            }
        }

        public bool HasSelection => SelectedItem != null;

        private string? _searchText;
        public string? SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                ItemsView?.Refresh();
            }
        }

        public string ActiveFilterText =>
            string.IsNullOrWhiteSpace(SearchText)
                ? "Showing all retail prices"
                : $"Search: {SearchText}";

        public RetailPricesListViewModel(IRetailPricesService service)
        {
            _service = service;
        }

        public async Task LoadAsync()
        {
            Items.Clear();

            var results = await _service.GetRetailPricesAsync();

            foreach (var r in results)
                Items.Add(r);

            ItemsView = CollectionViewSource.GetDefaultView(Items);
            ItemsView.Filter = FilterItems;

            OnPropertyChanged(nameof(ActiveFilterText));
        }

        private bool FilterItems(object obj)
        {
            if (obj is not RetailPriceListDto r)
                return false;

            if (string.IsNullOrWhiteSpace(SearchText))
                return true;

            return r.ItemTitle.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                || r.StoreName.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }

        public async Task DeleteSelectedAsync(string user)
        {
            if (SelectedItem == null)
                return;

            await _service.DeleteRetailPriceAsync(SelectedItem.RetailPriceId, user);

            Items.Remove(SelectedItem);
        }
    }
}
