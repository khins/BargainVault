using BargainVault.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Services
{
    public interface IRetailPricesService
    {
        Task<int> InsertRetailPriceAsync(RetailPriceDto dto, string enteredBy);
        Task UpdateRetailPriceAsync(RetailPriceDto dto, string enteredBy);
        Task DeleteRetailPriceAsync(int retailPriceId, string enteredBy);

        Task<RetailPriceDto> GetRetailPriceByIdAsync(int retailPriceId);
        Task<List<RetailPriceDto>> GetRetailPricesForItemAsync(int itemId);


    }

}
