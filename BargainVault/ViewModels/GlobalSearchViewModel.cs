using BargainVault.Domain.Models;
using BargainVault.Domain.Services;
using global::BargainVault.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BargainVault.ViewModels
{
        public class GlobalSearchViewModel : ViewModelBase
        {
            private readonly IGlobalSearchService _searchService;

            public ObservableCollection<GlobalSearchResultDto> Results { get; }
                = new ObservableCollection<GlobalSearchResultDto>();

            private string _searchText = string.Empty;
            public string SearchText
            {
                get => _searchText;
                set
                {
                    if (SetProperty(ref _searchText, value))
                    {
                        _ = PerformSearchAsync();
                    }
                }
            }

            private bool _isSearching;
            public bool IsSearching
            {
                get => _isSearching;
                private set => SetProperty(ref _isSearching, value);
            }

            public GlobalSearchViewModel(IGlobalSearchService searchService)
            {
                _searchService = searchService;
            }

            private async Task PerformSearchAsync()
            {
                Results.Clear();

                if (string.IsNullOrWhiteSpace(SearchText))
                    return;

                try
                {
                    IsSearching = true;

                    var results = await _searchService.SearchAsync(SearchText.Trim());

                    foreach (var result in results)
                        Results.Add(result);
                }
                finally
                {
                    IsSearching = false;
                }
            }
        }

}
