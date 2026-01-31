using BargainVault.Domain.Models;
using System;
using System.Threading.Tasks;

namespace BargainVault.Domain.Services
{
    public interface IAcquisitionsService
    {
        Task<int> InsertAcquisitionAsync(
            int itemId,
            string sourceType,
            int? auctionSiteId,
            DateTime? dateAcquired,
            int? qtyAcquired,
            decimal? unitHammerPrice,
            decimal? buyerPremium,
            decimal? taxRate,
            decimal? salesTaxPaid,
            decimal? totalSettlement,
            int? statusId,
            bool personal,
            bool businessExpense,
            string enteredBy);

        Task DeleteAcquisitionAsync(int acqId, string enteredBy);
        Task<IList<AcquisitionListDto>> GetAcquisitionsAsync();
        Task<AcquisitionDto> GetAcquisitionByIdAsync(int acqId);
        Task UpdateAcquisitionAsync(AcquisitionDto acquisition);

    }
}


