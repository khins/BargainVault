using BargainVault.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Services
{
    public interface ILiquidationService
    {
        Task<List<LiquidationCandidateDto>> GetCandidatesAsync();

        Task MarkDisregardAsync(int itemId, bool value);
    }
}
