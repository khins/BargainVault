using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.ViewModels.Base;
using System.Collections.ObjectModel;

namespace BargainVault.ViewModels
{  

    public class LiquidationViewModel : ViewModelBase
    {
        private readonly ILiquidationService _service;

        public ObservableCollection<LiquidationCandidateDto> Candidates { get; } = new();

        private LiquidationCandidateDto? _selectedCandidate;
        public LiquidationCandidateDto? SelectedCandidate
        {
            get => _selectedCandidate;
            set
            {
                SetProperty(ref _selectedCandidate, value);
                OnPropertyChanged(nameof(HasSelection));
            }
        }

        public bool HasSelection => SelectedCandidate != null;

        public LiquidationViewModel(ILiquidationService service)
        {
            _service = service;
        }

        public async Task LoadAsync()
        {
            var items = await _service.GetCandidatesAsync();

            Candidates.Clear();
            SelectedCandidate = null;

            foreach (var item in items)
            {
                Candidates.Add(item);
            }
        }

        public async Task MarkSelectedAsDisregardAsync()
        {
            if (SelectedCandidate == null)
                return;

            await _service.MarkDisregardAsync(SelectedCandidate.ItemId, true);
            await LoadAsync();
        }
    }
}
