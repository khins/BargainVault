using BargainVault.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Services
{
    public interface IReportsService
    {
        Task<List<AuctionProfitDto>> GetAuctionProfitAnalysisAsync();
    }

}
