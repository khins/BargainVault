using BargainVault.Domain.Models;
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
        Task<List<ItemDto>> GetItemsAsync();

        Task UpdateItemAsync(
            int itemId,
            int lotNumber,
            string title,
            string description,
            string imagePath,
            int quantity,
            string enteredBy
        );
    }
}
