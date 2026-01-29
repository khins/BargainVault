using BargainVault.Commands;
using BargainVault.Domain.Services;
using BargainVault.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BargainVault.ViewModels.Items
{
    public class ItemsEntryViewModel : ViewModelBase
    {
        private readonly IItemsService _itemsService;
        public RelayCommand SaveCommand { get; }

        public ItemsEntryViewModel(IItemsService itemsService)
        {
            _itemsService = itemsService;

            CreatedAt = DateTime.Now;

            SaveCommand = new RelayCommand(async () => await SaveAsync(), CanSave);
            NewEntryCommand = new RelayCommand(NewEntry);
            CloseCommand = new RelayCommand(Close);
        }

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
            var itemId = await _itemsService.InsertItemAsync(
                LotNumber,
                Title,
                Description,
                null,
                1,
                Environment.UserName
            );

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

        private void Close()
        {
            Close();
        }
    }

}
