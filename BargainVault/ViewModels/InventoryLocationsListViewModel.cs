using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.ViewModels.Base;
using BargainVault.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace BargainVault.ViewModels
{
    public class InventoryLocationsListViewModel : ViewModelBase
    {
        private readonly IInventoryLocationsService _service;

        public ObservableCollection<InventoryLocationListDto> Locations { get; }
            = new();

        public ICollectionView LocationsView { get; private set; }

        public InventoryLocationsListViewModel(IInventoryLocationsService service)
        {
            _service = service;
            _ = LoadAsync();
        }

        private InventoryLocationListDto? _selectedLocation;
        public InventoryLocationListDto? SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                SetProperty(ref _selectedLocation, value);
                OnPropertyChanged(nameof(HasSelection));
            }
        }

        public bool HasSelection => SelectedLocation != null;

        // 🔍 Search
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                LocationsView?.Refresh();
            }
        }

        public async Task LoadAsync()
        {
            Locations.Clear();

            var results = await _service.GetInventoryLocationsAsync();
            foreach (var loc in results)
                Locations.Add(loc);

            LocationsView = CollectionViewSource.GetDefaultView(Locations);
            LocationsView.Filter = FilterLocations;

            OnPropertyChanged(nameof(LocationsView));
        }

        private bool FilterLocations(object obj)
        {
            if (obj is not InventoryLocationListDto l)
                return false;

            if (string.IsNullOrWhiteSpace(SearchText))
                return true;

            return l.ItemTitle.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                || (l.BoothName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)
                || (l.StatusName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false);
        }

        public async Task DeleteSelectedAsync()
        {
            if (SelectedLocation == null)
                return;

            await _service.DeleteInventoryLocationAsync(
                SelectedLocation.InventoryLocationId,
                Environment.UserName);

            await LoadAsync();
        }



    }

}
