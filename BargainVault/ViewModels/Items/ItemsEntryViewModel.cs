using BargainVault.Commands;
using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BargainVault.ViewModels.Items
{
    public class ItemsEntryViewModel : ViewModelBase
    {
        private string _originalTitle = string.Empty;
        private string _originalDescription = string.Empty;
        private int? _originalLotNumber;
        private readonly IItemsService _itemsService;

        public ItemsEntryViewModel(IItemsService itemsService, ItemDto item)
            : this(itemsService)
        {
            ItemId = item.ItemId;
            LotNumber = item.LotNumber;
            Title = item.Title;
            Description = item.Description;
            CreatedAt = item.CreatedAt;
            ImagePath = item.ImagePath;

            CaptureOriginalValues();
        }

        public ItemsEntryViewModel(IItemsService itemsService)
        {
            _itemsService = itemsService;

            SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
            NewEntryCommand = new RelayCommand(NewEntry);
            CloseCommand = new RelayCommand(Close);
            LoadCommand = new RelayCommand(async () => await LoadAsync());

            CreatedAt = DateTime.Now;
            Title = string.Empty;
            Description = string.Empty;
            IsEditMode = false;

            CaptureOriginalValues();
        }

        public RelayCommand SaveCommand { get; }
        public RelayCommand LoadCommand { get; }
        public ICommand NewEntryCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand SelectImageCommand => new RelayCommand(SelectImage);

        private int _itemId;
        public int ItemId
        {
            get => _itemId;
            set
            {
                if (SetProperty(ref _itemId, value))
                {
                    OnPropertyChanged(nameof(HeaderText));
                }
            }
        }

        private bool _isDirty;
        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                if (SetProperty(ref _isDirty, value))
                {
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            private set => SetProperty(ref _isEditMode, value);
        }

        public ObservableCollection<ItemDto> Items { get; } = new();

        private ItemDto? _selectedItem;
        public ItemDto? SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        private int _lotNumber;
        public int LotNumber
        {
            get => _lotNumber;
            set
            {
                SetProperty(ref _lotNumber, value);
                UpdateDirtyState();
            }
        }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set
            {
                SetProperty(ref _title, value);
                SaveCommand.RaiseCanExecuteChanged();
                UpdateDirtyState();
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                SetProperty(ref _description, value);
                UpdateDirtyState();
            }
        }

        private DateTime? _createdAt;
        public DateTime? CreatedAt
        {
            get => _createdAt;
            set
            {
                if (SetProperty(ref _createdAt, value))
                {
                    OnPropertyChanged(nameof(CreatedAtDisplay));
                }

                OnPropertyChanged(nameof(DaysInInventoryDisplay));
            }
        }

        public string CreatedAtDisplay =>
            CreatedAt.HasValue ? CreatedAt.Value.ToString("MMM dd, yyyy") : "Not recorded";

        private string? _imagePath;
        public string? ImagePath
        {
            get => _imagePath;
            set
            {
                if (SetProperty(ref _imagePath, value))
                {
                    LoadImageFromPath();
                    IsDirty = true;
                }
            }
        }

        public string DaysInInventoryDisplay
        {
            get
            {
                if (!CreatedAt.HasValue)
                    return "Unknown";

                var days = (DateTime.Now.Date - CreatedAt.Value.Date).Days;
                return $"{days} day{(days == 1 ? "" : "s")}";
            }
        }

        public string HeaderText => ItemId > 0 ? $" ID: {ItemId}" : "Item Entry — New Item";

        private BitmapImage? _itemImage;
        public BitmapImage? ItemImage
        {
            get => _itemImage;
            private set => SetProperty(ref _itemImage, value);
        }

        private void CaptureOriginalValues()
        {
            _originalTitle = Title;
            _originalDescription = Description;
            _originalLotNumber = LotNumber;
            IsDirty = false;
        }

        private void LoadImageFromPath()
        {
            if (string.IsNullOrWhiteSpace(ImagePath) || !File.Exists(ImagePath))
            {
                ItemImage = null;
                return;
            }

            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(ImagePath, UriKind.Absolute);
                bitmap.EndInit();
                bitmap.Freeze();

                ItemImage = bitmap;
            }
            catch
            {
                ItemImage = null;
            }
        }

        private void UpdateDirtyState()
        {
            IsDirty =
                Title != _originalTitle ||
                Description != _originalDescription ||
                LotNumber != _originalLotNumber;

            SaveCommand.RaiseCanExecuteChanged();
        }

        private bool CanSave()
        {
            return IsDirty && !string.IsNullOrWhiteSpace(Title);
        }

        private async Task SaveAsync()
        {
            if (ItemId == 0)
            {
                await _itemsService.InsertItemAsync(
                    LotNumber,
                    Title,
                    Description,
                    ImagePath ?? string.Empty,
                    1,
                    Environment.UserName);
            }
            else
            {
                await _itemsService.UpdateItemAsync(
                    ItemId,
                    LotNumber,
                    Title,
                    Description,
                    ImagePath ?? string.Empty,
                    1,
                    Environment.UserName);
            }

            NewEntry();
        }

        private void NewEntry()
        {
            LotNumber = 0;
            Title = string.Empty;
            Description = string.Empty;
            CreatedAt = DateTime.Now;
            ImagePath = null;

            OnPropertyChanged(string.Empty);
        }

        public async Task LoadAsync()
        {
            Items.Clear();
            var items = await _itemsService.GetItemsAsync();
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        public async Task LoadAsync(int itemId)
        {
            var dto = await _itemsService.GetItemByIdAsync(itemId);
            if (dto == null)
                return;

            ItemId = dto.ItemId;
            LotNumber = dto.LotNumber;
            Title = dto.Title;
            Description = dto.Description;
            CreatedAt = dto.CreatedAt;
            ImagePath = dto.ImagePath;
            IsEditMode = true;
        }

        private void Close()
        {
            // Window closure is handled by the view.
        }

        private void SelectImage()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (dialog.ShowDialog() == true)
            {
                ImagePath = dialog.FileName;
                OnPropertyChanged(nameof(ItemImage));
            }
        }
    }
}
