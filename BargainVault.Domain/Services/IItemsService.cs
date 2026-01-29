using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Services
{
    public interface IItemsService
    {
        Task<int> InsertItemAsync(
            int lotNumber,
            string title,
            string description,
            string imagePath,
            int quantity,
            string enteredBy
        );

        Task<bool> TestConnectionAsync();
    }
}
