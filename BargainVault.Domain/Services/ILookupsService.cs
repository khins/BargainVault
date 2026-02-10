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
        Task<List<LookupDto>> GetAuctionSitesAsync();

        Task<List<LookupDto>> GetAcquisitionStatusesAsync();
        Task<List<ItemDto>> GetItemsAsync();
    }

}
