using BargainVault.Commands;
using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BargainVault.ViewModels.Items
{
    public class ItemsEntryViewModel : ViewModelBase
    {
        private string _originalTitle;
        private string _originalDescription;
        private int? _originalLotNumber;
        private readonly IItemsService _itemsService;
        public RelayCommand SaveCommand { get; }

        private int ItemId { get; set; }

        private bool _isDirty;
        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                if (SetProperty(ref _isDirty, value))
                    ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public ItemsEntryViewModel(IItemsService itemsService, ItemDto item) 
            : this(itemsService)
        {
            _itemsService = itemsService;

            ItemId = item.ItemId;
            LotNumber = item.LotNumber;
            Title = item.Title;
            Description = item.Description;
            CreatedAt = item.CreatedAt;
            ImagePath = item.ImagePath;
           
            SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
            NewEntryCommand = new RelayCommand(NewEntry);
            CloseCommand = new RelayCommand(Close);
            LoadCommand = new RelayCommand(async () => await LoadAsync());
            CaptureOriginalValues();
        }

        public ItemsEntryViewModel(IItemsService itemsService)
        {
            _itemsService = itemsService;

            CreatedAt = DateTime.Now;
            IsEditMode = false;

            SaveCommand = new RelayCommand(
                    async () => await SaveAsync(),
                    CanSave);
            NewEntryCommand = new RelayCommand(NewEntry);

            CaptureOriginalValues();
        }

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            private set => SetProperty(ref _isEditMode, value);
        }

        private void CaptureOriginalValues()
        {
            _originalTitle = Title;
            _originalDescription = Description;
            _originalLotNumber = LotNumber;
            IsDirty = false;
        }


        public ObservableCollection<ItemDto> Items { get; }
                = new ObservableCollection<ItemDto>();

        private ItemDto _selectedItem;
        public ItemDto SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public RelayCommand LoadCommand { get; }

        private int _lotNumber = 0;
        public int LotNumber
        {
            get => _lotNumber;
            set
            {
                SetProperty(ref _lotNumber, value);
                UpdateDirtyState();
            }
        }

        private string _title;
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

        private string _description;
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
            private set => SetProperty(ref _createdAt, value);
        }

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

        private void LoadImageFromPath()
        {
            if (string.IsNullOrWhiteSpace(ImagePath) 
                || !File.Exists(ImagePath))
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

        private BitmapImage? _itemImage;
        public BitmapImage? ItemImage
        {
            get => _itemImage;
            private set => SetProperty(ref _itemImage, value);
        }

        private void UpdateDirtyState()
        {
            IsDirty =
                Title != _originalTitle ||
                Description != _originalDescription ||
                LotNumber != _originalLotNumber;

            SaveCommand?.RaiseCanExecuteChanged();
        }


        public ICommand NewEntryCommand { get; }
        public ICommand CloseCommand { get; }

        private bool CanSave()
        {
            return IsDirty && !string.IsNullOrWhiteSpace(Title);
        }

        private async Task SaveAsync()
        {
            if (ItemId == 0)
            {

                var itemId = await _itemsService.InsertItemAsync(
                    LotNumber,
                    Title,
                    Description,
                    ImagePath,
                    1,
                    Environment.UserName
                );

            }
            else
            {
                await _itemsService.UpdateItemAsync(
                    ItemId,
                    LotNumber,
                    Title,
                    Description,
                    ImagePath,
                    1,
                    Environment.UserName
                );
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
                Items.Add(item);
        }

        public async Task LoadAsync(int itemId)
        {
            var dto = await _itemsService.GetItemByIdAsync(itemId);

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
            Close();
        }

        public ICommand SelectImageCommand => new RelayCommand(SelectImage);

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
