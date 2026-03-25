using BargainVault.Domain.Services;
using global::BargainVault.Commands;
using global::BargainVault.Domain.Models;
using global::BargainVault.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace BargainVault.ViewModels
{
    public class RetailPricesEntryViewModel : ViewModelBase
    {
        private readonly IRetailPricesService _retailPricesService;
        private readonly IItemsService _itemsService;
        private readonly IStoresService _storesService;
        private readonly RelayCommand _saveCommand;
        private readonly RelayCommand _closeCommand;

        private int? _retailPriceId;
        public int? RetailPriceId
        {
            get => _retailPriceId;
            set => SetProperty(ref _retailPriceId, value);
        }

        private bool _isDirty;

        public ObservableCollection<ItemDto> Items { get; } = new();
        public ObservableCollection<StoreDto> Stores { get; } = new();

        public RelayCommand SaveCommand => _saveCommand;
        public RelayCommand CloseCommand => _closeCommand;

        public RetailPricesEntryViewModel(
            IRetailPricesService retailPricesService,
            IItemsService itemsService,
            IStoresService storesService)
        {
            _retailPricesService = retailPricesService;
            _itemsService = itemsService;
            _storesService = storesService;
            _saveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
            _closeCommand = new RelayCommand(Close);
            PriceDate = DateTime.Today;
            _ = LoadLookupsAsync();
        }

        public RetailPricesEntryViewModel(
            IRetailPricesService retailPricesService,
            IItemsService itemsService,
            IStoresService storesService,
            RetailPriceDto dto)
        {
            _retailPricesService = retailPricesService;
            _itemsService = itemsService;
            _storesService = storesService;
            _saveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
            _closeCommand = new RelayCommand(Close);

            // Put the VM into edit mode using the existing record
            RetailPriceId = dto.RetailPriceId;

            SelectedItemId = dto.ItemId;
            SelectedStoreId = dto.StoreId;

            RetailPrice = dto.RetailPrice;
            PriceDate = dto.PriceDate;
            IsSalePrice = dto.IsSalePrice;
            Notes = dto.Notes;

            // This is an existing record so NOT dirty at load
            IsDirty = false;

            // Ensure Save button state updates
            SaveCommand?.RaiseCanExecuteChanged();
        }

        #region Properties

        private int _selectedItemId;
        public int SelectedItemId
        {
            get => _selectedItemId;
            set => SetDirty(ref _selectedItemId, value);
        }

        private int _selectedStoreId;
        public int SelectedStoreId
        {
            get => _selectedStoreId;
            set => SetDirty(ref _selectedStoreId, value);
        }

        private decimal _retailPrice;
        public decimal RetailPrice
        {
            get => _retailPrice;
            set => SetDirty(ref _retailPrice, value);
        }

        private DateTime? _priceDate;
        public DateTime? PriceDate
        {
            get => _priceDate;
            set => SetDirty(ref _priceDate, value);
        }

        private bool _isSalePrice;
        public bool IsSalePrice
        {
            get => _isSalePrice;
            set => SetDirty(ref _isSalePrice, value);
        }

        private string? _notes;
        public string? Notes
        {
            get => _notes;
            set => SetDirty(ref _notes, value);
        }

        public bool IsDirty
        {
            get => _isDirty;
            private set
            {
                _isDirty = value;
                OnPropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Load / Edit

        private async Task LoadLookupsAsync()
        {
            Items.Clear();
            Stores.Clear();

            foreach (var item in await _itemsService.GetItemsAsync())
                Items.Add(item);

            foreach (var store in await _storesService.GetStoresAsync())
                Stores.Add(store);
        }

        public async Task LoadRetailPriceAsync(int retailPriceId)
        {
            var dto = await _retailPricesService.GetRetailPriceByIdAsync(retailPriceId);
            if (dto == null)
                return;

            _retailPriceId = dto.RetailPriceId;

            SelectedItemId = dto.ItemId;
            SelectedStoreId = dto.StoreId;
            RetailPrice = dto.RetailPrice;
            PriceDate = dto.PriceDate;
            IsSalePrice = dto.IsSalePrice;
            Notes = dto.Notes;

            IsDirty = false;
        }

        #endregion

        #region Save Logic

        private async Task SaveAsync()
        {
            var dto = new RetailPriceDto
            {
                RetailPriceId = _retailPriceId ?? 0,
                ItemId = SelectedItemId,
                StoreId = SelectedStoreId,
                RetailPrice = RetailPrice,
                PriceDate = PriceDate,
                IsSalePrice = IsSalePrice,
                Notes = Notes
            };

            if (_retailPriceId == null)
            {
                var result = await _retailPricesService.InsertRetailPriceAsync(
                    dto,
                    Environment.UserName);

                MessageBox.Show(
                    $"Retail price saved (ID: {result})",
                    "Retail Price",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                await _retailPricesService.UpdateRetailPriceAsync(
                    dto,
                    Environment.UserName);

                MessageBox.Show(
                    "Retail price updated successfully.",
                    "Updated",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }

            IsDirty = false;
        }

        private bool CanSave()
        {
            return IsDirty
                && SelectedItemId > 0
                && SelectedStoreId > 0
                && RetailPrice > 0;
        }

        private void Close()
        {
            // Window is expected to bind this via CommandParameter
        }

        #endregion

        #region Helpers

        private void SetDirty<T>(ref T field, T value)
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged();
                IsDirty = true;
            }
        }

        #endregion
    }
}


