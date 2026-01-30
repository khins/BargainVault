using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.ViewModels.Base;
using System.Collections.ObjectModel;

namespace BargainVault.ViewModels.Items{


    namespace BargainVault.ViewModels.Items
    {
        public class ItemsListViewModel : ViewModelBase
        {
            private readonly IItemsService _itemsService;

            public ItemsListViewModel(IItemsService itemsService)
            {
                _itemsService = itemsService;
            }

            public ObservableCollection<ItemDto> Items { get; }
                = new ObservableCollection<ItemDto>();

            private ItemDto _selectedItem;

            public ItemDto SelectedItem
            {
                get => _selectedItem;
                set
                {
                    SetProperty(ref _selectedItem, value);
                    OnPropertyChanged(nameof(HasSelection));
                }
            }

            public bool HasSelection => SelectedItem != null;

            public async Task LoadAsync()
            {
                Items.Clear();

                var items = await _itemsService.GetItemsAsync();
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
        }
    }

}
