using BargainVault.Commands;
using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace BargainVault.ViewModels.Items
{
    public class ItemsEntryViewModel : ViewModelBase
    {
        private readonly IItemsService _itemsService;
        public RelayCommand SaveCommand { get; }

        private int ItemId { get; set; }

        public ItemsEntryViewModel(IItemsService itemsService, ItemDto item)
        {
            _itemsService = itemsService;

            ItemId = item.ItemId;
            LotNumber = item.LotNumber;
            Title = item.Title;
            Description = item.Description;
            CreatedAt = item.CreatedAt;
           
            SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
            NewEntryCommand = new RelayCommand(NewEntry);
            CloseCommand = new RelayCommand(Close);
            LoadCommand = new RelayCommand(async () => await LoadAsync());
        }

        public ItemsEntryViewModel(IItemsService itemsService)
        {
            _itemsService = itemsService;

            CreatedAt = DateTime.Now;
            IsEditMode = false;

            SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
            NewEntryCommand = new RelayCommand(NewEntry);
        }

        private bool IsEditMode { get; }


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
            set => SetProperty(ref _lotNumber, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                SetProperty(ref _title, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private DateTime _createdAt;
        public DateTime CreatedAt
        {
            get => _createdAt;
            private set => SetProperty(ref _createdAt, value);
        }

        public ICommand NewEntryCommand { get; }
        public ICommand CloseCommand { get; }

        private bool CanSave()
            => !string.IsNullOrWhiteSpace(Title);

        private async Task SaveAsync()
        {
            if (ItemId == 0)
            {

                var itemId = await _itemsService.InsertItemAsync(
                    LotNumber,
                    Title,
                    Description,
                    null,
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
                    null,
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

            OnPropertyChanged(string.Empty);
        }

        private async Task LoadAsync()
        {
            Items.Clear();
            var items = await _itemsService.GetItemsAsync();
            foreach (var item in items)
                Items.Add(item);
        }

        private void Close()
        {
            Close();
        }
    }

}
