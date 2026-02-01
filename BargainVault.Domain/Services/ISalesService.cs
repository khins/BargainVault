using BargainVault.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Services
{
    public interface ISalesService
    {
        Task<int> InsertSaleAsync(SaleDto dto, string enteredBy);
        Task UpdateSaleAsync(SaleDto dto, string enteredBy);
        Task DeleteSaleAsync(int saleId, string enteredBy);

        Task<SaleDto?> GetSaleByIdAsync(int saleId);
    }
}
