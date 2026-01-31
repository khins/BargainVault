using BargainVault.Commands;
using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
            set => SetProperty(ref _selectedAcquisition, value);
        }

        public ICommand RefreshCommand { get; }

        public async Task LoadAsync()
        {
            var results = await _service.GetAcquisitionsAsync();

            Acquisitions.Clear();
            foreach (var acq in results)
                Acquisitions.Add(acq);
        }
    }
}
