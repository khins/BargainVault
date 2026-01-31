using BargainVault.Commands;
using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.ViewModels.Base;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace BargainVault.ViewModels.Acquisitions
{
    public class AcquisitionsListViewModel : ViewModelBase
    {
        private readonly IAcquisitionsService _service;

        public AcquisitionsListViewModel(IAcquisitionsService service)
        {
            _service = service;
            Acquisitions = new ObservableCollection<AcquisitionListDto>();

            RefreshCommand = new RelayCommand(async () => await LoadAsync());
        }

        public ObservableCollection<AcquisitionListDto> Acquisitions { get; }

        private AcquisitionListDto _selectedAcquisition;
        public AcquisitionListDto SelectedAcquisition
        {
            get => _selectedAcquisition;
            set
            {
                SetProperty(ref _selectedAcquisition, value);
                OnPropertyChanged(nameof(HasSelection));
            }
        }
        public bool HasSelection => SelectedAcquisition != null;

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

        private ICollectionView? _itemsView;
        public ICollectionView? ItemsView
        {
            get => _itemsView;
            private set => SetProperty(ref _itemsView, value);
        }

        private bool FilterItems(object obj)
        {
            if (obj is not AcquisitionListDto item)
                return false;

            if (string.IsNullOrWhiteSpace(SearchText))
                return true;

            var text = SearchText.ToLower();

            return
                item.ItemTitle?.ToLower().Contains(text) == true;
        }

        private ObservableCollection<AcquisitionListDto> _items = new();
        public ObservableCollection<AcquisitionListDto> Items
        {
            get => _items;
            private set => SetProperty(ref _items, value);
        }

        public ICommand RefreshCommand { get; }

        public async Task LoadAsync()
        {
            var results = await _service.GetAcquisitionsAsync();

            Acquisitions.Clear();
            foreach (var acq in results)
                Acquisitions.Add(acq);

            Items = new ObservableCollection<AcquisitionListDto>(results);

            ItemsView = CollectionViewSource.GetDefaultView(Items);
            ItemsView.Filter = FilterItems;

            OnPropertyChanged(nameof(ItemsView));
        }
    }
}
