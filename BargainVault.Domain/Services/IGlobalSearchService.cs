using BargainVault.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Services
{
    public interface IGlobalSearchService
    {
        Task<List<GlobalSearchResultDto>> SearchAsync(string searchText);
    }

}
