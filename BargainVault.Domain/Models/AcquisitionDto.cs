using System;
using System.Collections.Generic;
using System.Text;

namespace BargainVault.Domain.Models
{
    public class AcquisitionDto
    {
        public int AcqId { get; set; }

        public int ItemId { get; set; }
        public string SourceType { get; set; }

        public int? AuctionSiteId { get; set; }
        public DateTime? DateAcquired { get; set; }

        public int QtyAcquired { get; set; }

        public decimal? UnitHammerPrice { get; set; }
        public decimal? BuyerPremium { get; set; }
        public decimal? TaxRate { get; set; }
        public decimal? SalesTaxPaid { get; set; }
        public decimal? TotalSettlement { get; set; }

        public int? StatusId { get; set; }

        public bool Personal { get; set; }
        public bool BusinessExpense { get; set; }
    }
}
