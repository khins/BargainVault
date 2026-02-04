using BargainVault.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Services
{
    public interface ILookupsService
    {
        Task<List<LookupDto>> GetBoothsAsync();
        Task<List<LookupDto>> GetInventoryStatusesAsync();
    }

}
