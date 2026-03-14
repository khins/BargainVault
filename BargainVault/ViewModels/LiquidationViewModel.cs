using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using BargainVault.ViewModels.Base;
using System.Collections.ObjectModel;

namespace BargainVault.ViewModels
{  

    public class LiquidationViewModel : ViewModelBase
    {
        private readonly ILiquidationService _service;

        public ObservableCollection<LiquidationCandidateDto> Candidates { get; set; }

        public LiquidationViewModel(ILiquidationService service)
        {
            _service = service;

            Candidates = new ObservableCollection<LiquidationCandidateDto>();
        }

        public async Task LoadAsync()
        {
            var items = await _service.GetCandidatesAsync();

            Candidates.Clear();

            foreach (var item in items)
            {
                Candidates.Add(item);
            }
        }
    }
}
