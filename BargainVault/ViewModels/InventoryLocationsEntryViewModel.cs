using BargainVault.Commands;
using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;

namespace BargainVault.ViewModels
{
    public class InventoryLocationsEntryViewModel : ViewModelBase
    {
        private readonly IInventoryLocationsService _inventoryService;
        private readonly IItemsService _itemsService;
        private readonly ILookupsService _lookupsService;

        private int? _inventoryLocationId;

        public ObservableCollection<ItemDto> Items { get; } = new();
        public ObservableCollection<LookupDto> Booths { get; } = new();
        public ObservableCollection<LookupDto> Statuses { get; } = new();
        public ObservableCollection<string> ChannelTypes { get; }
                = new ObservableCollection<string>();

        public RelayCommand SaveCommand { get; }
        public RelayCommand NewCommand { get; }

        public InventoryLocationsEntryViewModel(
            IInventoryLocationsService inventoryService,
            IItemsService itemsService,
            ILookupsService lookupsService)
        {
            _inventoryService = inventoryService;
            _itemsService = itemsService;
            _lookupsService = lookupsService;

            SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
            NewCommand = new RelayCommand(NewEntry);

            DatePlaced = DateTime.Now;

            _ = LoadLookupsAsync();
        }

        // Edit constructor
        public InventoryLocationsEntryViewModel(
            IInventoryLocationsService inventoryService,
            IItemsService itemsService,
            ILookupsService lookupsService,
            InventoryLocationDto dto)
            : this(inventoryService, itemsService, lookupsService)
        {
            _inventoryLocationId = dto.InventoryLocationId;

            SelectedItemId = dto.ItemId;
            SelectedBoothId = dto.BoothId;
            SelectedStatusId = dto.StatusId;
            DatePlaced = dto.DatePlaced;
            AskingPrice = dto.AskingPrice;
            Notes = dto.Notes;

            IsDirty = false;
        }

        // ──────────────────────────────
        // Properties
        // ──────────────────────────────

        public int? SelectedItemId { get; set; }

        private int? _selectedBoothId;
        public int? SelectedBoothId
        {
            get => _selectedBoothId;
            set
            {
                if (SetProperty(ref _selectedBoothId, value))
                    MarkDirty();
            }
        }

        public string? SelectedChannelType { get; set; }
        public DateTime? CreatedAt { get; set; }

        private int? _selectedStatusId;
        public int? SelectedStatusId
        {
            get => _selectedStatusId;
            set
            {
                if (SetProperty(ref _selectedStatusId, value))
                    MarkDirty();
            }
        }

        private DateTime? _datePlaced;
        public DateTime? DatePlaced
        {
            get => _datePlaced;
            set
            {
                if (SetProperty(ref _datePlaced, value))
                    MarkDirty();
            }
        }

        private decimal? _askingPrice;
        public decimal? AskingPrice
        {
            get => _askingPrice;
            set
            {
                if (SetProperty(ref _askingPrice, value))
                    MarkDirty();
            }
        }

        private string? _notes;
        public string? Notes
        {
            get => _notes;
            set
            {
                if (SetProperty(ref _notes, value))
                    MarkDirty();
            }
        }

        // ──────────────────────────────
        // Dirty tracking
        // ──────────────────────────────

        private bool _isDirty;
        public bool IsDirty
        {
            get => _isDirty;
            private set
            {
                SetProperty(ref _isDirty, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private void MarkDirty()
        {
            IsDirty = true;
        }

        // ──────────────────────────────
        // Commands
        // ──────────────────────────────

        private bool CanSave()
            => IsDirty && SelectedItemId > 0;

        private async Task SaveAsync()
        {
            if (SelectedItemId == null)
                return; // safety guard

            var dto = new InventoryLocationDto
            {
                InventoryLocationId = _inventoryLocationId ?? 0,
                ItemId = SelectedItemId.Value,          // ✅ explicit
                BoothId = SelectedBoothId,
                StatusId = SelectedStatusId,
                DatePlaced = DatePlaced,
                AskingPrice = AskingPrice,
                Notes = Notes
            };

            if (_inventoryLocationId == null)
            {
                _inventoryLocationId =
                    await _inventoryService.InsertInventoryLocationAsync(
                        dto,
                        Environment.UserName);

                CreatedAt = DateTime.Now;
                // ✅ CONFIRMATION POPUP
                MessageBox.Show(
                    $"Inventory location saved successfully.\n\nLocation ID: {_inventoryLocationId}",
                    "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                await _inventoryService.UpdateInventoryLocationAsync(
                    dto,
                    Environment.UserName);
            }

            IsDirty = false;
            SaveCommand.RaiseCanExecuteChanged();
        }


        // ──────────────────────────────
        // Lookup loading
        // ──────────────────────────────

        private async Task LoadLookupsAsync()
        {
            Items.Clear();
            Booths.Clear();
            Statuses.Clear();

            var items = await _itemsService.GetItemsAsync();
            foreach (var i in items)
                Items.Add(i);

            var booths = await _lookupsService.GetBoothsAsync();
            foreach (var b in booths)
                Booths.Add(b);

            Statuses.Clear();
            var statuses = await _lookupsService.GetInventoryStatusesAsync();
            foreach (var s in statuses)
                Statuses.Add(s);

            ChannelTypes.Clear();
            ChannelTypes.Add("In Booth");
            ChannelTypes.Add("Home Inventory");
            ChannelTypes.Add("Storage");

            MessageBox.Show($"ChannelTypes count: {ChannelTypes.Count}");

            Statuses.Clear();
            foreach (var status in await _lookupsService.GetInventoryStatusesAsync())
                Statuses.Add(status);

            MessageBox.Show($"Statuses count: {Statuses.Count}");
        }

        private void NewEntry()
        {
            _inventoryLocationId = null;

            SelectedItemId = null;
            SelectedBoothId = null;
            SelectedStatusId = null;
            SelectedChannelType = null;

            DatePlaced = DateTime.Today;
            AskingPrice = null;
            Notes = string.Empty;

            CreatedAt = null;

            IsDirty = false;

            SaveCommand.RaiseCanExecuteChanged();
        }

    }

}
