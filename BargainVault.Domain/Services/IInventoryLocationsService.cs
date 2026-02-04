using BargainVault.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Services
{
    public interface IInventoryLocationsService
    {
        Task<int> InsertInventoryLocationAsync(
            InventoryLocationDto dto,
            string enteredBy);

        Task UpdateInventoryLocationAsync(
            InventoryLocationDto dto,
            string enteredBy);

        Task DeleteInventoryLocationAsync(
            int inventoryLocationId,
            string enteredBy);

        Task<List<InventoryLocationListDto>> GetInventoryLocationsAsync();

        Task<InventoryLocationDto?> GetInventoryLocationByIdAsync(
            int inventoryLocationId);
    }


}
