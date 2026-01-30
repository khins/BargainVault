using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.ViewModels.Base;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

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


            private ObservableCollection<ItemDto> _items = new();

            public ObservableCollection<ItemDto> Items
            {
                get => _items;
                private set => SetProperty(ref _items, value);
            }

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

            public ICollectionView ItemsView { get; private set; }


            private string _searchText;
            public string SearchText
            {
                get => _searchText;
                set
                {
                    SetProperty(ref _searchText, value);
                    ItemsView?.Refresh();
                }
            }

            public bool HasSelection => SelectedItem != null;

            private bool FilterItems(object obj)
            {
                if (obj is not ItemDto item)
                    return false;

                if (string.IsNullOrWhiteSpace(SearchText))
                    return true;

                return item.Title?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true;
            }


            public async Task LoadAsync()
            {
                var results = await _itemsService.GetItemsAsync();

                Items = new ObservableCollection<ItemDto>(results);

                ItemsView = CollectionViewSource.GetDefaultView(Items);
                ItemsView.Filter = FilterItems;

                OnPropertyChanged(nameof(ItemsView));
            }
        }
    }

}
